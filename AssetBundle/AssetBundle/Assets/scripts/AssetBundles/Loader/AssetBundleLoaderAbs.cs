using AssetBundles.data;
using System.ComponentModel;
using UnityEngine;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 加载状态
    /// </summary>
    public enum LoadState
    {
        [Description("初始状态")]
        State_None = 0,
        [Description("加载状态")]
        State_Loading = 1,
        [Description("加载失败")]
        State_Error = 2,
        [Description("加载完成")]
        State_Complete = 3
    }

    /// <summary>
    /// 加载基类
    /// </summary>
    public abstract class AssetBundleLoaderAbs
    {
        internal LoadState state = LoadState.State_None;
        internal AssetBundleInfo bundleInfo;
        internal DependInfo bundleData;
        internal CallBackLoaderComplete loaderComplete;

        /// <summary>
        /// 正式加载
        /// </summary>
        public virtual void LoadBundle()
        {

        }

        /// <summary>
        /// 是否加载完成
        /// </summary>
        public virtual bool isComplete
        {
            get { return state == LoadState.State_Error || state == LoadState.State_Complete; }
        }

        protected virtual void Complete()
        {
            if (loaderComplete != null)
            {
                var handler = loaderComplete;
                loaderComplete = null;
                handler(bundleInfo);
            }
        }

        protected virtual void Error()
        {
            if (loaderComplete != null)
            {
                var handler = loaderComplete;
                loaderComplete = null;
                handler(bundleInfo);
            }
        }

        /// <summary>
        /// 创建内容
        /// </summary>
        /// <param name="assetBundle"></param>
        protected void CreateBundleInfo(AssetBundle assetBundle = null)
        {
            if (bundleInfo == null) bundleInfo = new AssetBundleInfo();
            bundleInfo.bundle = assetBundle;
            bundleInfo.bundleData = bundleData;
        }
    }
}