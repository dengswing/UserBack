using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace AssetBundles.Loader
{
    public class WWWManager
    {
        AssetBundleManager bundleManager;

        public WWWManager(AssetBundleManager bundleManager)
        {
            this.bundleManager = bundleManager;
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="name"></param>
        /// <param name="isLocal">是否检查读本地缓存</param>
        public void LoadFile(Action<string> callBack, string name, bool isLocal = true)
        {
            LoaderFile(name, callBack, isLocal);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="backComplete"></param>
        /// <param name="path"></param>
        /// <param name="version"></param>
        public void LoadAssetBundle(Action<AssetBundle> backComplete, string path, int version)
        {
            bundleManager.StartCoroutine(LoaderManager(backComplete, path, version));
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="backComplete"></param>
        /// <param name="path"></param>
        /// <param name="version"></param>
        public void LoadAssetBundle(Action<AssetBundle> backComplete, string path, Hash128 version)
        {
            bundleManager.StartCoroutine(LoaderManager(backComplete, path, version));
        }

        #region loader manager
        IEnumerator LoaderManager(Action<AssetBundle> backComplete, string path, int version)
        {
            var www = WWW.LoadFromCacheOrDownload(path, version);

#if DEBUG_CONSOLE
            UnityEngine.Debug.LogFormat("LoaderManager::path={0}|version={1}", www.url, version);
#endif
            yield return www;
            if (www.error != null)
            {
#if DEBUG_CONSOLE
                UnityEngine.Debug.LogFormat("LoaderManager::Error");
#endif
                yield return null;
            }

            backComplete(www.assetBundle);

            www.Dispose();
            www = null;
        }

        IEnumerator LoaderManager(Action<AssetBundle> backComplete, string path, Hash128 version)
        {
            var www = WWW.LoadFromCacheOrDownload(path, version);

#if DEBUG_CONSOLE
            UnityEngine.Debug.LogFormat("LoaderManager::path={0}|version={1}", www.url, version.ToString());
#endif
            yield return www;
            if (www.error != null)
            {
#if DEBUG_CONSOLE
                UnityEngine.Debug.LogFormat("LoaderManager::Error！path={0}",www.url);
#endif
                yield return null;
            }

            backComplete(www.assetBundle);

            www.Dispose();
            www = null;
        }

        #endregion

        #region load file
        void LoaderFile(string name, Action<string> callBack, bool isLocal = true)
        {
            var platfrom = PathGlobal.GetPlatformFile();
            var serverPath = PathGlobal.GetStreamingAssetsSourceFile(platfrom);
            var localPath = PathGlobal.GetPersistentDataPathSourceFile(platfrom);

            serverPath = PathGlobal.GetJoinPath(serverPath, name);
            localPath = PathGlobal.GetJoinPath(localPath, name);

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoaderFile:: serverPath=" + serverPath + "|localPath=" + localPath);
#endif

            if (isLocal && File.Exists(localPath))
                LocalLoderFile(localPath, callBack);
            else
                bundleManager.StartCoroutine(ServerLoaderFile(serverPath, callBack, name));
        }

        /// <summary>
        /// 本地加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        void LocalLoderFile(string path, Action<string> callBack)
        {
            var stream = new StreamReader(path, System.Text.Encoding.UTF8);

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LocalLoderFile:: url=" + path);
#endif
            string allContent = stream.ReadToEnd();

            stream.Dispose();
            stream.Close();
            stream = null;

            callBack(allContent);
        }

        /// <summary>
        /// 加载服务器资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        IEnumerator ServerLoaderFile(string path, Action<string> callBack,string name)
        {
            WWW www = new WWW(path);

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("ServerLoader:: url=" + www.url);
#endif
            yield return www;
            if (www.error != null)
            {

#if DEBUG_CONSOLE
                UnityEngine.Debug.Log("ServerLoader:: Error! url=" + www.url);
#endif
                yield return null;
            }

            var allContent = www.text;

            ReplaceLocalRes(www, name);
            www = null;

            callBack(allContent);
        }

        void ReplaceLocalRes(WWW www,string name)
        {
            if (www == null || www.bytes == null) return;

            var path = PathGlobal.GetJoinPath(PathGlobal.GetPlatformFile(), name);
            path = PathGlobal.GetPersistentDataPathSourceFile(path);

            var deposit = path.Substring(0, path.LastIndexOf("/"));

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("ReplaceLocalRes:: deposit=" + deposit);
#endif

            if (!Directory.Exists(deposit))
            {
                Directory.CreateDirectory(deposit);
            }

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("ReplaceLocalRes:: path=" + path);
#endif

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
#if DEBUG_CONSOLE
                UnityEngine.Debug.Log("ReplaceLocalRes:: url=" + path);
#endif
                byte[] data = www.bytes;
                stream.Write(data, 0, data.Length);

#if DEBUG_CONSOLE
                UnityEngine.Debug.Log("ReplaceLocalRes:: write success");
#endif
                www.Dispose();
                www = null;
            }
        }

        #endregion

    }
}