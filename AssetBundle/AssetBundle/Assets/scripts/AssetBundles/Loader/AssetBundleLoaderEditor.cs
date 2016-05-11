using AssetBundles.data;
using UnityEditor;
using UnityEngine;

namespace AssetBundles.Loader
{
    /// <summary>
    /// 编辑器加载
    /// </summary>
    public class AssetBundleLoaderEditor : AssetBundleLoaderAbs
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
                Complete();
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

        protected override void Complete()
        {
            state = LoadState.State_Complete;

            bundleInfo = new EditorInfo();
            CreateBundleInfo();

            base.Complete();
        }

        protected override void Error()
        {
            state = LoadState.State_Error;
            base.Error();
        }
    }
    class EditorInfo : AssetBundleInfo
    {
        protected override T GetAsset<T>(string path)
        {
            var index = path.IndexOf(".");
            if (index != -1) path = path.Substring(0, index);

            T data = default(T);
            try
            {
                data = Resources.Load<T>(path);
            }
            catch (System.Exception)
            {
                data = (T)AssetDatabase.LoadMainAssetAtPath(path);
            }

            return data;
        }
    }
}