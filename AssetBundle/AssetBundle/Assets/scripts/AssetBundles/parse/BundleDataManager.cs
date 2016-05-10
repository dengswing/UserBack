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
        /// <summary>
        /// bundle依赖信息
        /// </summary>
       public Dictionary<string, DependInfo> dependInfo { get; private set; }

        /// <summary>
        /// 主bundle
        /// </summary>
        public AssetBundleManifest manifest { get; private set; }

        /// <summary>
        /// 解析json数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseDepend(string value)
        {
            dependInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, DependInfo>>(value);
        }

        /// <summary>
        /// 解析manifest数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseManifest(AssetBundleManifest value)
        {
            manifest = value;
        }
    }
}
