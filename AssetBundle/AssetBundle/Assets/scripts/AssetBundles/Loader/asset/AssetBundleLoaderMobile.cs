using UnityEngine;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 手机加载
    /// </summary>
    public class AssetBundleLoaderMobile : AssetBundleLoaderAbs
    {
        protected AssetBundle bundle;

        /// <summary>
        /// 开始加载
        /// </summary>
        public override void LoadBundle()
        {
            base.LoadBundle();

            if (state == LoadState.State_None)
            {
                state = LoadState.State_Loading;
                LoaderBundle();
            }
            else if (state == LoadState.State_Error)
            {
                Error();
            }
            else if (state == LoadState.State_Complete)
            {
                Complete();
            }
        }

        void LoaderBundle()
        {
            var platfrom = PathGlobal.GetPlatformFile();
            var path = PathGlobal.GetStreamingAssetsSourceFile(platfrom);
            path = PathGlobal.GetJoinPath(path, bundleData.bundleName);
            Hash128 version = bundleManager.GetBundleHash(bundleData.bundleName);
            bundleManager.LoaderManager((AssetBundle asset) =>
            {
                bundle = asset;
                Complete();
            }, path, version);
        }

        protected override void Complete()
        {
            state = LoadState.State_Complete;
            if (bundle != null) CreateBundleInfo(bundle);
            bundle = null;
            base.Complete();
        }

        protected override void Error()
        {
            state = LoadState.State_Error;
            base.Error();
        }
    }
}