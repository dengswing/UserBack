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
    /// 队列加载
    /// </summary>
    public class QueueLoaderBundle
    {
        #region property

        /// <summary>
        /// 已经在加载的
        /// </summary>
        Dictionary<string, AssetBundleLoaderAbs> loaderCache;

        /// <summary>
        /// 同时最大的加载数
        /// </summary>
        const int MAX_REQUEST = 3;

        /// <summary>
        /// 可再次申请的加载数
        /// </summary>
        int requestRemain = MAX_REQUEST;
        /// <summary>
        /// 当前申请要加载的队列
        /// </summary>
        Queue<AssetBundleLoaderAbs> requestQueue;

        /// <summary>
        ///所有的完成加载回调
        /// </summary>
        Action allFinishBack;

        AssetBundleManager bundleManager;

        #endregion

        public QueueLoaderBundle(AssetBundleManager bundleManager)
        {
            this.bundleManager = bundleManager;
            requestQueue = new Queue<AssetBundleLoaderAbs>();
            loaderCache = new Dictionary<string, AssetBundleLoaderAbs>();
        }

        public void Load(DependInfo info, CallBackLoaderComplete handler)
        {
            AssetBundleLoaderAbs loader = CreateLoader(info);

            if (loader.isComplete)
            {
                if (handler != null) handler(loader.bundleInfo);
            }
            else
            {
                if (handler != null) loader.loaderComplete += handler;
                RequestLoadBundle(loader);
            }
        }

        AssetBundleLoaderAbs CreateLoader(DependInfo info)
        {
            var name = info.bundleName;
            if (loaderCache.ContainsKey(name))
            {
                return loaderCache[name];
            }

            AssetBundleLoaderAbs loader;
#if UNITY_EDITOR
            loader = new AssetBundleLoaderMobile();
#else
            loader = new AssetBundleLoaderMobile(); 
#endif

            loader.bundleManager = bundleManager;
            loader.bundleData = info;

            loaderCache[name] = loader;
            return loader;
        }

        //开始加载
        internal void RequestLoadBundle(AssetBundleLoaderAbs loader)
        {
            if (requestRemain < 0) requestRemain = 0;

            if (requestRemain == 0)
            {
                requestQueue.Enqueue(loader);
            }
            else
            {
                LoadBundle(loader);
            }
        }

        void LoadBundle(AssetBundleLoaderAbs loader)
        {
            if (!loader.isComplete)
            {
                requestRemain--;
                loader.LoadBundle();
            }
        }

        internal void LoadComplete(AssetBundleLoaderAbs loader)
        {
#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadComplete:: loader finish=" + loader.bundleName);
#endif

            requestRemain++;
            AssetBundleLoaderAbs nextBundle = requestQueue.Dequeue();
            if (null != nextBundle) RequestLoadBundle(nextBundle);
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
