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

        /// <summary>
        /// 队列加载
        /// </summary>
        QueueLoaderBundle queueLoader;

        /// <summary>
        /// 解析器
        /// </summary>
        BundleDataManager dataManager;

        /// <summary>
        /// 初始化完成加载回调
        /// </summary>
        Action initFinishBack;

        #endregion


        public AssetBundleManager()
        {
            dataManager = new BundleDataManager();
            queueLoader = new QueueLoaderBundle(this);
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
        /// <param name="finishBack">加载完成回调</param>
        /// <param name="version">文件的版本号</param>
        public void LoadAssetBundle(Action finishBack, int version = 1)
        {
            var platfrom = PathGlobal.GetPlatformFile();
            var path = PathGlobal.GetStreamingAssetsSourceFile(platfrom);
            path = PathGlobal.GetJoinPath(path, PathGlobal.DEPEND_FILE);
            LoadAssetBundle(path, finishBack, version);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">加载的文件路径</param>
        /// <param name="finishBack">加载完成回调</param>
        /// <param name="version">文件的版本号</param>
        public void LoadAssetBundle(string path, Action finishBack, int version = 1)
        {
            if (null != initFinishBack || null == finishBack)
            {
                return;
            }

            initFinishBack = finishBack;
            LoaderDependJson(ParseDepend, path, version);
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

        void ParseDepend(string value, int version)
        {
            dataManager.ParseDepend(value);
            var path = PathGlobal.GetStreamingAssetsSourceFile(PathGlobal.GetPlatformFile()); //bundle main url
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
        }

        #region loader resource
        /// <summary>
        /// 加载depend.json
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        void LoaderDependJson(Action<string, int> callBack, string path, int version)
        {
            StartCoroutine(LoaderManager((WWW www) =>
            {
                var value = www.url;
                callBack(value, version);

            }, path, version));
        }

        /// <summary>
        /// 加载manifest
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        void LoaderBundleManifest(Action<AssetBundleManifest> callBack, string path, int version = 1)
        {
            StartCoroutine(LoaderManager((WWW www) =>
            {
                var manifestBundle = www.assetBundle;
                var manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
                callBack(manifest);

            }, path, version));
        }

        #endregion

        #region loader manager

        IEnumerator LoaderManager(Action<WWW> callBack, string path, int version)
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

            callBack(www);

            www.Dispose();
            www = null;
        }

        public IEnumerator LoaderManager(Action<WWW> callBack, string path, Hash128 version)
        {
            var www = WWW.LoadFromCacheOrDownload(path, version);

#if DEBUG_CONSOLE
            UnityEngine.Debug.LogFormat("LoaderManager::path={0}|version={1}", www.url, version.ToString());
#endif
            yield return www;
            if (www.error != null)
            {
#if DEBUG_CONSOLE
                UnityEngine.Debug.LogFormat("LoaderManager::Error");
#endif
                yield return null;
            }

            callBack(www);

            www.Dispose();
            www = null;
        }

        #endregion
    }
}
