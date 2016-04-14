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

    public abstract class AssetBundleLoaderAbs
    {
        public string bundleName;
        public LoadState state = LoadState.State_None;
        public AssetBundleManager bundleManager;
        public AssetBundleData bundleData;
        public AssetBundleInfo bundleInfo;

        public virtual void Load()
        {

        }

        /// <summary>
        /// 其它都准备好了，加载AssetBundle
        /// 注意：这个方法只能被 AssetBundleManager 调用
        /// 由 Manager 统一分配加载时机，防止加载过卡
        /// </summary>
        public virtual void LoadBundle()
        {

        }

        public virtual bool isComplete
        {
            get
            {
                return state == LoadState.State_Error || state == LoadState.State_Complete;
            }
        }

        protected virtual void Complete()
        {
            //if (onComplete != null)
            //{
            //    var handler = onComplete;
            //    onComplete = null;
            //    handler(bundleInfo);
            //}
            bundleManager.LoadComplete(this);
        }

        protected virtual void Error()
        {
            //if (onComplete != null)
            //{
            //    var handler = onComplete;
            //    onComplete = null;
            //    handler(bundleInfo);
            //}
            bundleManager.LoadError(this);
        }

        /// <summary>
        /// 创建内容
        /// </summary>
        /// <param name="assetBundle"></param>
        protected void CreateBundleInfo(AssetBundle assetBundle = null)
        {
            if (bundleInfo == null) bundleInfo = new AssetBundleInfo();
            bundleInfo.bundleName = bundleName;
            bundleInfo.bundle = assetBundle;
            bundleInfo.bundleData = bundleData;
        }
    }
}