using System.Collections.Generic;
using UnityEngine;

namespace AssetBundles.data
{
    /// <summary>
    /// 每个bundle的信息
    /// </summary>
    public class AssetBundleInfo
    {
        //数据
        internal DependInfo bundleData;

        //资源bundle
        internal AssetBundle bundle;

        //缓存资源
        Dictionary<string, Object> bundlesCacheRes = new Dictionary<string, Object>();

        /// <summary>
        /// 返回bundle名
        /// </summary>
        public string BundleName { get { return (null != bundleData ? bundleData.bundleName : string.Empty); } }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="path">素材路径</param>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : Object
        {
            return GetAsset<T>(path);
        }

        protected virtual T GetAsset<T>(string path) where T : Object
        {
            T data = default(T);
            if (bundle == null) return data;

            if (bundlesCacheRes.ContainsKey(path))
            {
                data = (T)bundlesCacheRes[path];
            }
            else if (bundle.Contains(path))
            {
                try
                {
                    data = (T)bundle.LoadAsset(path);
                    bundlesCacheRes.Add(path, data);
                }
                catch (System.Exception)
                {

                }
            }
            return data;
        }

    }
}