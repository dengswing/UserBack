using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundles
{  
    /// <summary>
    /// 每个bundle的信息
    /// </summary>
    public class AssetBundleInfo
    {
        public string bundleName;
        public AssetBundleData bundleData;

        internal AssetBundle bundle;
        Dictionary<string, Object> bundlesCacheRes;

        public Object LoadAsset(string name)
        {
            if (bundle == null) return null;

            if (bundle.Contains(name))
                return bundle.LoadAsset(name);
            else
                return null;
        }

    }
}