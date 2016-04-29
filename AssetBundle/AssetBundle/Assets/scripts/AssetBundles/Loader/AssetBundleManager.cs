using System.Collections.Generic;
using Utilities;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 加载总管理
    /// </summary>
    public class AssetBundleManager : SingleInstance<AssetBundleManager>
    {
        /// <summary>
        /// 正在加载的缓存
        /// </summary>
        Dictionary<string, AssetBundleLoaderAbs> loaderCache = new Dictionary<string, AssetBundleLoaderAbs>();

        /// <summary>
        /// 同时最大的加载数
        /// </summary>
        private const int MAX_REQUEST = 3;
        /// <summary>
        /// 可再次申请的加载数
        /// </summary>
        private int _requestRemain = MAX_REQUEST;
        /// <summary>
        /// 当前申请要加载的队列
        /// </summary>
        private Queue<AssetBundleLoaderAbs> _requestQueue = new Queue<AssetBundleLoaderAbs>();


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
#elif UNITY_IOS
            return new AssetBundleLoaderMobile(); 
#else
            return new AssetBundleLoaderMobile();
#endif
        }

        internal void LoadComplete(AssetBundleLoaderAbs loader)
        {

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadComplete:: loader finish="+ loader.bundleName);
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
    }
}
