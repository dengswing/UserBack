using AssetBundles.data;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundles.parse
{
    /// <summary>
    /// 数据管理
    /// </summary>
    public class BundleDataManager
    {
        Dictionary<string, string> assetBundleDepend;

        #region get set
        /// <summary>
        /// asset依赖信息
        /// </summary>
        public Dictionary<string, DependInfo> DependInfo { get; private set; }

        /// <summary>
        /// 主asset
        /// </summary>
        public AssetBundleManifest Manifest { get; private set; }

        /// <summary>
        /// asset数据
        /// </summary>
        public Dictionary<string, AssetBundleInfo> AssetBundleInfoData { get; private set; }
        #endregion
        
        public BundleDataManager()
        {
            AssetBundleInfoData = new Dictionary<string, AssetBundleInfo>();
        }
     
        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public T GetAsset<T>(string name) where T : Object
        {
            return GetAssetBundle<T>(name);
        }

        T GetAssetBundle<T>(string name) where T : Object
        {
            if (null == assetBundleDepend || !assetBundleDepend.ContainsKey(name)) return default(T);

            T data = default(T);
            var bundleName = assetBundleDepend[name];
            if (AssetBundleInfoData.ContainsKey(bundleName))
            {
                data = AssetBundleInfoData[bundleName].LoadAsset<T>(name);
            }

            return data;
        }

        /// <summary>
        /// 解析json数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseDepend(string value)
        {
            DependInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, DependInfo>>(value);

            if (null == assetBundleDepend) assetBundleDepend = new Dictionary<string, string>();
            assetBundleDepend.Clear();

            foreach (var item in DependInfo)
            {
                var depend = item.Value;
                foreach (var assetName in depend.binding)
                {
                    assetBundleDepend.Add(assetName, depend.bundleName);
                }
            }
        }

        /// <summary>
        /// 解析manifest数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseManifest(AssetBundleManifest value)
        {
            Manifest = value;
        }

        /// <summary>
        /// 添加bundle内容
        /// </summary>
        /// <param name="data"></param>
        internal void AddAssetBundleInfo(AssetBundleInfo data)
        {
            if (data == null) return;
            var assetName = data.BundleName;
            if (!AssetBundleInfoData.ContainsKey(assetName))
            {
                AssetBundleInfoData.Add(assetName, data);
            }
            else
            {
                AssetBundleInfoData[assetName] = data;
            }
        }
    }
}
