﻿using AssetBundles.data;
using AssetBundles.Loader;
using AssetBundles.parse;
using System;
using System.Collections;
using UnityEngine;
using Utilities;

using Object = UnityEngine.Object;

namespace AssetBundles
{
    /// <summary>
    /// 加载总管理
    /// </summary>
    public class AssetBundleManager : SingleInstance<AssetBundleManager>, IAssetBundleManager
    {
        #region property       

        //队列加载
        QueueLoaderBundleFile queueLoader;

        //解析器
        BundleDataManager dataManager;

        //加载器
        WWWManager wwwManager;

        //初始化完成加载回调
        Action initFinishBack;

        #endregion

        public AssetBundleManager()
        {
            dataManager = new BundleDataManager(this);
            queueLoader = new QueueLoaderBundleFile(this);
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
        public Object LoadAsset(string name)
        {
            return LoadAsset<Object>(name);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public T LoadAsset<T>(string name) where T : Object
        {
            return dataManager.LoadAsset<T>(name);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public T LoadAssetBundleAsync<T>(string name, CallBackAssetComplete<T> finishBack = null) where T : Object
        {
            return dataManager.LoadAssetBundleAsync<T>(name, finishBack);
        }

        /// <summary>
        /// 获取bundle的hash
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Hash128 GetBundleHash(string name)
        {
            return dataManager.Maifest.GetAssetBundleHash(name);
        }

        internal void LoaderManager(Action<AssetBundle> callBack, string path, Hash128 version)
        {
            wwwManager.LoadAssetBundle(callBack, path, version);
        }

        #region loader maifest
        void ParseDepend(string value)
        {
            dataManager.ParseDepend(value);

            var version = dataManager.MaifestVersion;

            var platform = PathGlobal.GetPlatformFile();
            var path = PathGlobal.GetStreamingAssetsSourceFile(platform); //bundle main url
            path = PathGlobal.GetJoinPath(path, platform);

            LoaderBundleManifest((AssetBundleManifest maifest) =>
            {
                dataManager.ParseManifest(maifest);
                LoaderAllBundle();
            }, path, version);
        }
        #endregion

        #region loader all bundle
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
        #endregion

        #region loader resource
        /// <summary>
        /// 加载depend.json
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        void LoaderDependJson(Action<string> callBack, bool isHaveUpdate)
        {
            var name = PathGlobal.DEPEND_FILE;
            wwwManager.LoadFile((string data) =>
            {
                callBack(data);
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

                var manifest = (asset ? asset.LoadAsset<AssetBundleManifest>("AssetBundleManifest") : null);
                callBack(manifest);

            }, path, version);
        }
        #endregion
    }
}