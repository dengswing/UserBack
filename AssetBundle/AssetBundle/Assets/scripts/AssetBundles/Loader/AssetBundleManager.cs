using AssetBundles.data;
using AssetBundles.parse;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 加载总管理
    /// </summary>
    public class AssetBundleManager : SingleInstance<AssetBundleManager>
    {
        #region property
        /// <summary>
        /// 正在加载的缓存
        /// </summary>
        Dictionary<string, AssetBundleLoaderAbs> loaderCache = new Dictionary<string, AssetBundleLoaderAbs>();

        /// <summary>
        /// 同时最大的加载数
        /// </summary>
        const int MAX_REQUEST = 3;
        /// <summary>
        /// 可再次申请的加载数
        /// </summary>
        int _requestRemain = MAX_REQUEST;
        /// <summary>
        /// 当前申请要加载的队列
        /// </summary>
        Queue<AssetBundleLoaderAbs> _requestQueue = new Queue<AssetBundleLoaderAbs>();

        /// <summary>
        /// 解析器
        /// </summary>
        BundleDataManager parseManager;

        /// <summary>
        /// 初始化完成加载回调
        /// </summary>
        Action initFinishBack;

        #endregion

        #region get set property

        #endregion

        public AssetBundleManager()
        {
            parseManager = new BundleDataManager();
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="path">加载的文件路径</param>
        /// <param name="finishBack">加载完成回调</param>
        /// <param name="version">文件的版本号</param>
        public void StartLoad(string path, Action finishBack, int version = 1)
        {
            if (null != initFinishBack || null == finishBack)
            {
                return;
            }

            initFinishBack = finishBack;
            StartCoroutine(LoaderDependJson(ParseDepend, path, version));
        }

        void ParseDepend(string value, int version)
        {
            parseManager.ParseDepend(value);

            var path = PathGlobal.GetStreamingAssetsSourceFile(PathGlobal.GetPlatformFile()); //bundle main url

            StartCoroutine(LoaderBundleManifest((AssetBundleManifest maifest) =>
            {
                parseManager.ParseManifest(maifest);
                                
            }, path, version));
        }



        public void Load(string path, CallBackLoaderComplete handler = null)
        {
            AssetBundleLoaderAbs loader = this.CreateLoader(path);
            if (loader == null)
            {
                if (handler != null) handler(null);
            }
            else if (loader.isComplete)
            {
                if (handler != null) handler(loader.bundleInfo);
            }
            else
            {
                if (handler != null) loader.loaderComplete += handler;
                loader.Load();
            }
        }

        //开始加载
        internal void RequestLoadBundle(AssetBundleLoaderAbs loader)
        {
            if (_requestRemain < 0) _requestRemain = 0;

            if (_requestRemain == 0)
            {
                _requestQueue.Enqueue(loader);
            }
            else
            {
                this.LoadBundle(loader);
            }
        }

        void LoadBundle(AssetBundleLoaderAbs loader)
        {
            if (!loader.isComplete)
            {
                _requestRemain--;
                loader.LoadBundle();
            }
        }

        AssetBundleLoaderAbs CreateLoader(string path)
        {
            path = path.ToLower();

            AssetBundleLoaderAbs loader = null;

            if (loaderCache.ContainsKey(path))
            {
                loader = loaderCache[path];
            }
            else
            {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
                loader = CreateLoader();
                loader.bundleManager = this;
                loader.bundleName = path;
#else
                AssetBundleData data = _depInfoReader.GetAssetBundleInfo(path);
                if (data == null)
                {
                    return null;
                }
                loader = this.CreateLoader();
                loader.bundleManager = this;
                loader.bundleData = data;
                loader.bundleName = data.fullName;
#endif
                loaderCache[path] = loader;
            }
            return loader;
        }

        protected virtual AssetBundleLoaderAbs CreateLoader()
        {
#if UNITY_EDITOR
            return new AssetBundleLoaderMobile();
#else
            return new AssetBundleLoaderMobile(); 
#endif
        }

        internal void LoadComplete(AssetBundleLoaderAbs loader)
        {

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadComplete:: loader finish=" + loader.bundleName);
#endif
            if (loaderCache.ContainsKey(loader.bundleName))
            {
                loaderCache.Remove(loader.bundleName);
            }

            _requestRemain++;
        }

        internal void LoadError(AssetBundleLoaderAbs loader)
        {
#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadError:: loader finish=" + loader.bundleName);
#endif
            LoadComplete(loader);
        }


        /// <summary>
        /// 加载depend.json
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        IEnumerator LoaderDependJson(Action<string, int> callBack, string path, int version)
        {
            var www = WWW.LoadFromCacheOrDownload(path, version);

#if DEBUG_CONSOLE
            UnityEngine.Debug.LogFormat("LoaderDependJson::path={0}|version={1}", www.url, www.GetHashCode());
#endif
            yield return www;
            if (www.error != null)
            {
#if DEBUG_CONSOLE
                UnityEngine.Debug.LogFormat("LoaderDependJson::Error");
#endif
                yield return null;
            }

            var value = www.text;
            callBack(value, version);
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 加载manifest
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        IEnumerator LoaderBundleManifest(Action<AssetBundleManifest> callBack, string path, int version = 1)
        {
            var www = WWW.LoadFromCacheOrDownload(path, version);

#if DEBUG_CONSOLE
            UnityEngine.Debug.LogFormat("LoaderBundleManifest::path={0}|version={1}", www.url, version);
#endif
            yield return www;
            if (www.error != null)
            {
#if DEBUG_CONSOLE
                UnityEngine.Debug.LogFormat("LoaderBundleManifest::Error");
#endif
                yield return null;
            }

            var manifestBundle = www.assetBundle;
            var manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
            callBack(manifest);
            www.Dispose();
            www = null;
        }
    }
}
