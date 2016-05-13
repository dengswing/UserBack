#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace AssetBundles.data
{
    class EditorInfo : AssetBundleInfo
    {
        protected override T LoadAssetBundle<T>(string path)
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
#if UNITY_EDITOR
                data = (T)AssetDatabase.LoadMainAssetAtPath(path);
#endif
            }
            return data;
        }
    }
}
