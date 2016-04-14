using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 加载总管理
    /// </summary>
    public class AssetBundleManager : MonoBehaviour
    {
        public CallBackLoaderComplete loaderComplete;

        Dictionary<string, AssetBundleLoaderAbs> loaderCache = new Dictionary<string, AssetBundleLoaderAbs>();

        internal AssetBundleLoaderAbs CreateLoader(string path)
        {
            path = path.ToLower();

            AssetBundleLoaderAbs loader = null;

            if (loaderCache.ContainsKey(path))
            {
                loader = loaderCache[path];
            }
            else
            {
#if !AB_MODE && UNITY_EDITOR
                loader = CreateLoader();
                loader.bundleManager = this;
                loader.bundleName = path;
#else
                AssetBundleData data = _depInfoReader.GetAssetBundleInfo(path);
                if (data == null)
                {
                    return null;
                }
                loader = this.CreateLoader();
                loader.bundleManager = this;
                loader.bundleData = data;
                loader.bundleName = data.fullName;
#endif
                loaderCache[path] = loader;
            }

            return loader;
        }

        protected virtual AssetBundleLoaderAbs CreateLoader()
        {
#if UNITY_EDITOR && AB_MODE
            return new AssetBundleLoaderMobile();
#elif UNITY_IOS
            return new IOSAssetBundleLoader();            
#elif UNITY_EDITOR
            return new AssetBundleLoaderMobile();
#else
            return new AssetBundleLoaderMobile();
#endif
        }

        public void LoadComplete(AssetBundleLoaderAbs loader)
        {
            loaderComplete(loader.bundleInfo);
        }

        public void LoadError(AssetBundleLoaderAbs loader)
        {
            LoadComplete(loader);
        }

    }
}
