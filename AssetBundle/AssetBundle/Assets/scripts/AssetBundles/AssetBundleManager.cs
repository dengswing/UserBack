using AssetBundles.data;
using AssetBundles.Loader;
using AssetBundles.parse;
using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace AssetBundles
{
    /// <summary>
    /// 加载总管理
    /// </summary>
    public class AssetBundleManager : SingleInstance<AssetBundleManager>, IAssetBundleManager
    {
        #region property       

        //队列加载
        QueueLoaderBundle queueLoader;

        //解析器
        BundleDataManager dataManager;

        //加载器
        WWWManager wwwManager;

        //初始化完成加载回调
        Action initFinishBack;

        #endregion


        public AssetBundleManager()
        {
            dataManager = new BundleDataManager();
            queueLoader = new QueueLoaderBundle(this);
            wwwManager = new WWWManager(this);
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerUrl
        {
            get { return PathGlobal.ServerURL; }
            set { PathGlobal.ServerURL = value; }
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="finishBack">加载所有资源完成回调</param>
        /// <param name="isHaveUpdate">是否有更新</param>
        public void LoadAssetBundle(Action finishBack, bool isHaveUpdate = false)
        {
            if (null != initFinishBack || null == finishBack)
            {
                return;
            }

            initFinishBack = finishBack;
            LoaderDependJson(ParseDepend, isHaveUpdate);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public UnityEngine.Object GetAsset(string name)
        {
            return GetAsset<UnityEngine.Object>(name);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public T GetAsset<T>(string name) where T : UnityEngine.Object
        {
            return dataManager.GetAsset<T>(name);
        }

        /// <summary>
        /// 获取bundle的hash
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Hash128 GetBundleHash(string name)
        {
            return dataManager.Manifest.GetAssetBundleHash(name);
        }

        internal void LoaderManager(Action<AssetBundle> callBack, string path, Hash128 version)
        {
            wwwManager.LoadAssetBundle(callBack, path, version);
        }

        void ParseDepend(string value, int version)
        {
            dataManager.ParseDepend(value);

            var platform = PathGlobal.GetPlatformFile();
            var path = PathGlobal.GetStreamingAssetsSourceFile(platform); //bundle main url
            path = PathGlobal.GetJoinPath(path, platform);
            LoaderBundleManifest((AssetBundleManifest maifest) =>
            {
                dataManager.ParseManifest(maifest);
                LoaderAllBundle();
            }, path, version);
        }

        void LoaderAllBundle()
        {
            queueLoader.Load(dataManager.DependInfo, CompleteHandler);
        }

        void CompleteHandler(AssetBundleInfo data)
        {
            dataManager.AddAssetBundleInfo(data);

            StopCoroutine("LoadFinishCheck");
            StartCoroutine("LoadFinishCheck");
        }

        IEnumerator LoadFinishCheck()
        {
            yield return new WaitForSeconds(.1f);

            if (queueLoader.IsFinishAllLoad && initFinishBack != null)
            {
                initFinishBack();
                initFinishBack = null;
            }
        }

        #region loader resource
        /// <summary>
        /// 加载depend.json
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        void LoaderDependJson(Action<string, int> callBack, bool isHaveUpdate)
        {
            var name = PathGlobal.DEPEND_FILE;
            wwwManager.LoadFile((string data) =>
            {
                callBack(data, 1); //to du 1 version
            }, name, !isHaveUpdate);
        }

        /// <summary>
        /// 加载manifest
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        void LoaderBundleManifest(Action<AssetBundleManifest> callBack, string path, int version = 1)
        {
            wwwManager.LoadAssetBundle((AssetBundle asset) =>
            {
                var manifest = asset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                callBack(manifest);

            }, path, version);
        }

        #endregion
    }
}
