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
        /// 解析json数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseDepend(string value)
        {
            DependInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, DependInfo>>(value);
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
