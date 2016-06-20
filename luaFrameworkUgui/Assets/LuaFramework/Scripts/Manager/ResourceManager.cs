#if ASYNC_MODE
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

public class AssetBundleInfo
{
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle)
    {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 0;
    }
}

namespace LuaFramework
{

    public class ResourceManager : Manager
    {
        string m_BaseDownloadingURL = "";
        AssetBundleManifest m_AssetBundleManifest = null;
        Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
        Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();
        Dictionary<string, int> loadingAssetBundles = new Dictionary<string, int>();

        class LoadAssetRequest
        {
            public Type assetType;
            public string[] assetNames;
            public LuaFunction luaFunc;
            public Action<UObject[]> sharpFunc;
        }

        // Load AssetBundleManifest.
        public void Initialize(string manifestName, Action initOK)
        {
            m_BaseDownloadingURL = Util.GetRelativePath();

            LoadAsset<AssetBundleManifest>(manifestName, new string[] { "AssetBundleManifest" }, delegate (UObject[] objs)
            {
                if (objs.Length > 0)
                {
                    m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                }
                if (initOK != null) initOK();
            });
        }

        public void LoadPrefab(string abName, string assetName, Action<UObject[]> func)
        {
            LoadAsset<GameObject>(abName, new string[] { assetName }, func);
        }

        public void LoadPrefab(string abName, string[] assetNames, Action<UObject[]> func)
        {
            LoadAsset<GameObject>(abName, assetNames, func);
        }

        public void LoadPrefab(string abName, string[] assetNames, LuaFunction func)
        {
            LoadAsset<GameObject>(abName, assetNames, null, func);
        }

        public void LoadPathPrefab(string path, Action<UObject[]> func)
        {
            LoadPathPrefab<GameObject>(path, func);
        }

        public void LoadPathPrefab<T>(string path, Action<UObject[]> func) where T : UObject
        {
            LoadAssetPrefab<T>(path, func, null);
        }

        public void LoadPathPrefab(string path, LuaFunction func)
        {
            LoadAssetPrefab<GameObject>(path, null, func);
        }

        internal string GetAssetName(string path)
        {
            string assetName;
            string directory;
            string ext;
            GetBundlePath(path, out assetName, out ext, out directory);
            return assetName;
        }

        void LoadAssetPrefab<T>(string path, Action<UObject[]> action, LuaFunction func) where T : UObject
        {
            string assetName;
            string directory;
            string ext;
            var abName = GetBundlePath(path, out assetName, out ext, out directory);
#if UNITY_EDITOR
            if (AppConst.DebugMode)
            {
                LoadResrouces<T>(abName, new string[] { assetName }, action, func, directory, ext);
            }
            else
            {
                LoadAsset<T>(abName, new string[] { assetName }, action, func);
            }
#else
            LoadAsset<T>(abName, new string[] { assetName }, action, func);
#endif
        }

        string GetBundlePath(string path, out string assetName, out string ext, out string directory)
        {
            assetName = string.Empty;
            directory = string.Empty;
            ext = ".prefab";
            string bundleName = string.Empty;

            var content = path.Split(new char[] { '/', '.' });
            var count = content.Length;
            if (count == 0)
            {
                Debug.LogError("Introduced into the wrong path!");
                return string.Empty;
            }

            for (int i = 0; i < count; i++)
            {
                switch (i)
                {
                    case 0:
                        directory = content[i];
                        break;
                    case 1:
                        bundleName = content[i].ToLower() + AppConst.ExtName;
                        break;
                    case 2:
                        assetName = content[i];
                        break;
                    case 3:
                        ext = content[i];
                        break;
                }
            }

            return bundleName;
        }

        string GetRealAssetPath(string abName)
        {
            if (abName.Equals(AppConst.PlatformFile))
            {
                return abName;
            }
            abName = abName.ToLower();
            if (!abName.EndsWith(AppConst.ExtName))
            {
                abName += AppConst.ExtName;
            }
            if (abName.Contains("/"))
            {
                return abName;
            }
            if (m_AssetBundleManifest == null) return null;
            string[] paths = m_AssetBundleManifest.GetAllAssetBundles();
            for (int i = 0; i < paths.Length; i++)
            {
                int index = paths[i].LastIndexOf('/');
                string path = paths[i].Remove(0, index + 1);
                if (path.Equals(abName))
                {
                    return paths[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        void LoadAsset<T>(string abName, string[] assetNames, Action<UObject[]> action = null, LuaFunction func = null) where T : UObject
        {
            abName = GetRealAssetPath(abName);
            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = typeof(T);
            request.assetNames = assetNames;
            request.luaFunc = func;
            request.sharpFunc = action;

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests))
            {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                StartCoroutine(OnLoadAsset<T>(abName));
            }
            else {
                requests.Add(request);
            }
        }

        void LoadResrouces<T>(string abName, string[] assetNames, Action<UObject[]> action = null, LuaFunction func = null, string directory = "Builds", string ext = ".prefab") where T : UObject
        {
            for (int j = 0; j < assetNames.Length; j++)
            {
                string assetPath = assetNames[j];
                T data = Resources.Load<T>(assetPath);
#if UNITY_EDIOR
                if (null == data)
                {
                    Debug.LogFormat("load {0} resources Error", assetPath);

                    var path = string.Format("{0}/{1}/{2}/{3}{4}", AppConst.AssetBundlePath, directory, abName.Replace(AppConst.ExtName, ""), assetPath, ext);
                    data = (T)UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);

                    if (data == null) Debug.LogFormat("load {0} resources Error", path);
                }
#else
                List<UObject> result = new List<UObject>();
                result.Add(data);

                if (action != null)
                {
                    action(result.ToArray());
                    action = null;
                }
                if (func != null)
                {
                    func.Call((object)result.ToArray());
                    func.Dispose();
                    func = null;
                }
            }
#endif
        }

        IEnumerator OnLoadAsset<T>(string abName) where T : UObject
        {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                yield return StartCoroutine(OnLoadAssetBundle(abName, typeof(T)));

                bundleInfo = GetLoadedAssetBundle(abName);

                if (bundleInfo == null)
                {
                    m_LoadRequests.Remove(abName);
                    yield break;
                }
            }
            List<LoadAssetRequest> list = null;
            if (!m_LoadRequests.TryGetValue(abName, out list))
            {
                m_LoadRequests.Remove(abName);
                yield break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] assetNames = list[i].assetNames;
                List<UObject> result = new List<UObject>();

                AssetBundle ab = bundleInfo.m_AssetBundle;
                for (int j = 0; j < assetNames.Length; j++)
                {
                    string assetPath = assetNames[j];
                    AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                    yield return request;
                    result.Add(request.asset);

                    //T assetObj = ab.LoadAsset<T>(assetPath);
                    //result.Add(assetObj);
                }
                if (list[i].sharpFunc != null)
                {
                    list[i].sharpFunc(result.ToArray());
                    list[i].sharpFunc = null;
                }
                if (list[i].luaFunc != null)
                {
                    list[i].luaFunc.Call((object)result.ToArray());
                    list[i].luaFunc.Dispose();
                    list[i].luaFunc = null;
                }
                bundleInfo.m_ReferencedCount++;
            }
            m_LoadRequests.Remove(abName);
        }

        IEnumerator OnLoadAssetBundle(string abName, Type type)
        {
            string url = m_BaseDownloadingURL + abName;

            WWW download = null;
            if (type == typeof(AssetBundleManifest))
            {
                Debug.LogFormat("WWW Path={0}", url);
                download = new WWW(url);
            } else {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
                if (dependencies.Length > 0)
                {
                    m_Dependencies.Add(abName, dependencies);
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        string depName = dependencies[i];
                        AssetBundleInfo bundleInfo = null;
                        if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                        {
                            bundleInfo.m_ReferencedCount++;
                        }
                        else if (!m_LoadRequests.ContainsKey(depName))
                        {
                            yield return StartCoroutine(OnLoadAssetBundle(depName, type));
                        }
                    }
                }

                Debug.LogFormat("WWW.LoadFromCacheOrDownload Path={0}", url);

                if (!loadingAssetBundles.ContainsKey(url))
                {
                    download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(abName), 0);
                    loadingAssetBundles.Add(url, 1);
                }
                else
                {
                    yield break;
                }
            }
            yield return download;

            AssetBundle assetObj = download.assetBundle;
            if (assetObj != null)
            {
                m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
            }
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName)
        {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null) return null;
            }
            return bundle;
        }

        public void UnloadAssetBundle(string abName)
        {
            abName = GetRealAssetPath(abName);
            if (string.IsNullOrEmpty(abName)) return;

            Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + abName);
            UnloadAssetBundleInternal(abName);
            UnloadDependencies(abName);
            Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
        }

        void UnloadDependencies(string abName)
        {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return;

            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }
            m_Dependencies.Remove(abName);
        }

        void UnloadAssetBundleInternal(string abName)
        {
            AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
            if (bundle == null) return;

            if (--bundle.m_ReferencedCount == 0)
            {
                bundle.m_AssetBundle.Unload(false);
                m_LoadedAssetBundles.Remove(abName);
                Debug.Log(abName + " has been unloaded successfully");
            }
        }
    }
}
#else

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaFramework;
using LuaInterface;
using UObject = UnityEngine.Object;

namespace LuaFramework {
    public class ResourceManager : Manager {
        private string[] m_Variants = { };
        private AssetBundleManifest manifest;
        private AssetBundle shared, assetbundle;
        private Dictionary<string, AssetBundle> bundles;

        void Awake() {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize() {
            byte[] stream = null;
            string uri = string.Empty;
            bundles = new Dictionary<string, AssetBundle>();
            uri = Util.DataPath + AppConst.AssetDir;
            if (!File.Exists(uri)) return;
            stream = File.ReadAllBytes(uri);
            assetbundle = AssetBundle.CreateFromMemoryImmediate(stream);
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        public T LoadAsset<T>(string abname, string assetname) where T : UnityEngine.Object {
            abname = abname.ToLower();
            AssetBundle bundle = LoadAssetBundle(abname);
            return bundle.LoadAsset<T>(assetname);
        }

        public void LoadPrefab(string abName, string[] assetNames, LuaFunction func) {
            abName = abName.ToLower();
            List<UObject> result = new List<UObject>();
            for (int i = 0; i < assetNames.Length; i++) {
                UObject go = LoadAsset<UObject>(abName, assetNames[i]);
                if (go != null) result.Add(go);
            }
            if (func != null) func.Call((object)result.ToArray());
        }

        /// <summary>
        /// 载入AssetBundle
        /// </summary>
        /// <param name="abname"></param>
        /// <returns></returns>
        public AssetBundle LoadAssetBundle(string abname) {
            if (!abname.EndsWith(AppConst.ExtName)) {
                abname += AppConst.ExtName;
            }
            AssetBundle bundle = null;
            if (!bundles.ContainsKey(abname)) {
                byte[] stream = null;
                string uri = Util.DataPath + abname;
                Debug.LogWarning("LoadFile::>> " + uri);
                LoadDependencies(abname);

                stream = File.ReadAllBytes(uri);
                bundle = AssetBundle.CreateFromMemoryImmediate(stream); //关联数据的素材绑定
                bundles.Add(abname, bundle);
            } else {
                bundles.TryGetValue(abname, out bundle);
            }
            return bundle;
        }

        /// <summary>
        /// 载入依赖
        /// </summary>
        /// <param name="name"></param>
        void LoadDependencies(string name) {
            if (manifest == null) {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }
            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = manifest.GetAllDependencies(name);
            if (dependencies.Length == 0) return;

            for (int i = 0; i < dependencies.Length; i++)
                dependencies[i] = RemapVariantName(dependencies[i]);

            // Record and load all dependencies.
            for (int i = 0; i < dependencies.Length; i++) {
                LoadAssetBundle(dependencies[i]);
            }
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        string RemapVariantName(string assetBundleName) {
            string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

            // If the asset bundle doesn't have variant, simply return.
            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
                return assetBundleName;

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++) {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit) {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }
            if (bestFitIndex != -1)
                return bundlesWithVariant[bestFitIndex];
            else
                return assetBundleName;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        void OnDestroy() {
            if (shared != null) shared.Unload(true);
            if (manifest != null) manifest = null;
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}
#endif
