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
            return LoadAssetBundle<T>(path);
        }

        /// <summary>
        /// 替换路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal string ReplacePath(string path)
        {
            path = path.ToLower();
            return string.Format("assets/resources/{0}", path);
        }

        /// <summary>
        /// 获取缓存内的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        internal T GetCacheAsset<T>(string path) where T : Object
        {
            if (bundlesCacheRes.ContainsKey(path))
            {
                return (T)bundlesCacheRes[path];
            }
            return null;
        }

        /// <summary>
        /// 保存资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal T SetAssetBundle<T>(Object asset, string path) where T : Object
        {
            T data = GetCacheAsset<T>(path);
            if (data == null)
            {
                data = (T)asset;
                bundlesCacheRes.Add(path, data);
            }

            return data;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual T LoadAssetBundle<T>(string path) where T : Object
        {
            T data = default(T);
            if (bundle == null) return data;
            path = ReplacePath(path);

            data = GetCacheAsset<T>(path);
            if (data == null && bundle.Contains(path))
            {
                try
                {
                    data = (T)bundle.LoadAsset(path);
                    bundlesCacheRes.Add(path, data);
                }
                catch (System.Exception)
                {
#if DEBUG_CONSOLE
                    UnityEngine.Debug.Log("GetAsset:: Error! path=" + path);
#endif
                }
            }

            return data;
        }
    }
}