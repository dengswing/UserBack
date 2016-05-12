using AssetBundles.data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 队列加载
    /// </summary>
    public class QueueLoaderAsset
    {
        #region property

        //请求加载素材
        Dictionary<string, LoaderAssetData> loadAsset;

        //同时最大的加载数
        const int MAX_REQUEST = 5;

        //可再次申请的加载数
        int requestRemain = MAX_REQUEST;

        //当前申请要加载的队列
        Queue<LoaderAssetData> requestQueue;

        AssetBundleManager bundleManager;

        #endregion

        public QueueLoaderAsset(AssetBundleManager bundleManager)
        {
            this.bundleManager = bundleManager;
            requestQueue = new Queue<LoaderAssetData>();
            loadAsset = new Dictionary<string, LoaderAssetData>();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="info"></param>
        /// <param name="path"></param>
        /// <param name="finishBack"></param>
        public void LoadAssetBundleAsync(AssetBundleInfo info, string path, Action<Object> finishBack)
        {
            var loadData = new LoaderAssetData();
            loadData.info = info;
            loadData.path = path;
            loadData.finishBack = finishBack;

            RequestLoadBundle(loadData);
        }

        #region queue loader
        void RequestLoadBundle(LoaderAssetData loader, bool isNext = false)
        {
            var id = loader.id;
            if (loadAsset.ContainsKey(id))
            {
                if (loadAsset[id].finishBack == null)
                    loadAsset[id].finishBack = loader.finishBack;

                loadAsset[id].finishBack += loader.finishBack;
                return;
            }

           if(!isNext) loadAsset.Add(id, loader);

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

        void LoadBundle(LoaderAssetData loader)
        {
            requestRemain--;
            bundleManager.StartCoroutine(LoadAssetBundle(loader, LoadComplete));
        }
        
        IEnumerator LoadAssetBundle(LoaderAssetData loader, Action<LoaderAssetData, Object> finishBack)
        {
            AssetBundleInfo info = loader.info;
            var path = loader.path;
            path = info.ReplacePath(path);
            var data = info.GetCacheAsset<Object>(path);
            if (data != null)
            {
                finishBack(loader, data);
                yield break;
            }

            var bundle = info.bundle;
            var request = bundle.LoadAssetAsync(path);
            yield return request;
            data = info.SetAssetBundle<Object>(request.asset, path);

           // yield return new WaitForSeconds(3f);
            finishBack(loader, data);
        }
        #endregion

        #region loader callback
        void LoadComplete(LoaderAssetData loader, Object data)
        {
#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LoadAssetBundleAsync:: finish=" + loader.path);
#endif
            requestRemain++;

            if (requestQueue.Count > 0)
            {
                LoaderAssetData nextBundle = requestQueue.Dequeue();
                if (null != nextBundle) RequestLoadBundle(nextBundle, true);
            }

            loader.finishBack(data);
        }

        #endregion
    }

    class LoaderAssetData
    {
        public AssetBundleInfo info;
        public string path;
        public Action<Object> finishBack;

        public bool Equals(LoaderAssetData data)
        {
            return (info.Equals(data.info) && path.Equals(data.path));
        }

        /// <summary>
        /// 唯一id
        /// </summary>
        public string id{ get { return info.BundleName + "_" + path; } }
    }
}
