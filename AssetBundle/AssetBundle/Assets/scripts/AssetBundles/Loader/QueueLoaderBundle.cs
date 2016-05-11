using AssetBundles.data;
using System;
using System.Collections.Generic;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 队列加载
    /// </summary>
    public class QueueLoaderBundle
    {
        #region property

        //已经在加载的
        Dictionary<string, AssetBundleLoaderAbs> loaderCache;

        //同时最大的加载数
        const int MAX_REQUEST = 3;

        //可再次申请的加载数
        int requestRemain = MAX_REQUEST;

        //当前申请要加载的队列
        Queue<AssetBundleLoaderAbs> requestQueue;

        //所有的完成加载回调
        Action allFinishBack;

        AssetBundleManager bundleManager;

        //完成回调
        Dictionary<string, CallBackLoaderComplete> dComplete;
        #endregion

        public QueueLoaderBundle(AssetBundleManager bundleManager)
        {
            this.bundleManager = bundleManager;
            requestQueue = new Queue<AssetBundleLoaderAbs>();
            loaderCache = new Dictionary<string, AssetBundleLoaderAbs>();
            dComplete = new Dictionary<string, CallBackLoaderComplete>();
        }

        #region get set
        /// <summary>
        /// 申请加载的总数
        /// </summary>
        public int LoadAssetCount { get; private set; }

        /// <summary>
        /// 当前已经加载好的数量
        /// </summary>
        public int LoadAssetCurrent { get; private set; }

        /// <summary>
        /// 是否完成所有加载
        /// </summary>
        public bool IsFinishAllLoad { get { return LoadAssetCount == LoadAssetCurrent; } }
        #endregion

        /// <summary>
        /// 加载所有的
        /// </summary>
        /// <param name="infoAll"></param>
        /// <param name="handler"></param>
        public void Load(Dictionary<string, DependInfo> infoAll, CallBackLoaderComplete handler)
        {
            foreach (var item in infoAll)
            {
                Load(item.Value, handler);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="info"></param>
        /// <param name="handler"></param>
        public void Load(DependInfo info, CallBackLoaderComplete handler)
        {
            AssetBundleLoaderAbs loader = CreateLoader(info);

            if (loader.IsComplete)
            {
                if (handler != null) handler(loader.bundleInfo);
            }
            else
            {
                if (!dComplete.ContainsKey(info.bundleName))
                {
                    dComplete.Add(info.bundleName, handler);
                }
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
            loader = new AssetBundleLoaderEditor();
#else
            loader = new AssetBundleLoaderMobile(); 
#endif

            loader.bundleManager = bundleManager;
            loader.bundleData = info;
            loader.loaderComplete = LoadComplete;
            loader.loaderError = LoadError;

            loaderCache[name] = loader;
            return loader;
        }

        #region queue loader
        void RequestLoadBundle(AssetBundleLoaderAbs loader, bool isNext = false)
        {
            if (!isNext) LoadAssetCount++; //计算加载数量

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
            if (!loader.IsComplete)
            {
                requestRemain--;
                loader.LoadBundle();
            }
        }
        #endregion

        #region loader callback
        void LoadComplete(AssetBundleLoaderAbs loader)
        {
            var assetName = loader.BundleName;
#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadComplete:: loader finish=" + assetName);
#endif

            LoadAssetCurrent++;
            requestRemain++;

            if (requestQueue.Count > 0)
            {
                AssetBundleLoaderAbs nextBundle = requestQueue.Dequeue();
                if (null != nextBundle) RequestLoadBundle(nextBundle, true);
            }

            if (dComplete.ContainsKey(assetName) && dComplete[assetName] != null)
            {
                dComplete[assetName](loader.bundleInfo);
                dComplete[assetName] = null;
                dComplete.Remove(assetName);
            }
        }

        void LoadError(AssetBundleLoaderAbs loader)
        {
#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadError:: loader finish=" + loader.BundleName);
#endif
            LoadComplete(loader);
        }
        #endregion
    }
}
