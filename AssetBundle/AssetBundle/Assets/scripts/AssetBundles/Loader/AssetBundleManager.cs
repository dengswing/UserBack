using AssetBundles.parse;
using System;
using System.Collections;
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
        /// 解析器
        /// </summary>
        BundleDataManager dataManager;

        /// <summary>
        /// 初始化完成加载回调
        /// </summary>
        Action initFinishBack;

        #endregion

        #region get set property

        #endregion

        public AssetBundleManager()
        {
            dataManager = new BundleDataManager();
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
            LoaderDependJson(ParseDepend, path, version);
        }

        /// <summary>
        /// 获取bundle的hash
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Hash128 GetBundleHash(string name)
        {
            return dataManager.manifest.GetAssetBundleHash(name);
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
            var dInfo = dataManager.dependInfo;
            foreach (var item in dInfo)
            {

            }
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
                var value = www.text;
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
