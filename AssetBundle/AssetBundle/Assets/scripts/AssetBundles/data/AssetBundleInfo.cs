using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundles.data
{
    /// <summary>
    /// 每个bundle的信息
    /// </summary>
    public class AssetBundleInfo
    {
        //数据
        public DependInfo bundleData;

        //资源bundle
        internal AssetBundle bundle;

        //缓存资源
        Dictionary<string, Object> bundlesCacheRes = new Dictionary<string, Object>();

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Object LoadAsset(string name)
        {
            if (bundle == null) return null;            

            if (bundlesCacheRes.ContainsKey(name))
            {
                return bundlesCacheRes[name];
            }
            else if (bundle.Contains(name))
            {
                bundlesCacheRes.Add(name, bundle.LoadAsset(name));
                return bundlesCacheRes[name];
            }
            else
            {
                return null;
            }
        }

    }
}