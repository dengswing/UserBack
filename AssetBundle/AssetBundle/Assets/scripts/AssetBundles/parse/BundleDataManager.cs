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

        VersionInfo assetVersion;

        #region get set
        /// <summary>
        /// asset依赖信息
        /// </summary>
        public Dictionary<string, DependInfo> DependInfo { get { return (null != assetVersion ? assetVersion.dDependInfo : null); } }

        public int MaifestVersion { get { return (null != assetVersion ? assetVersion.maifestVersion : 1); } }

        /// <summary>
        /// 主asset
        /// </summary>
        public AssetBundleManifest Maifest { get; private set; }

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

        T GetAssetBundle<T>(string path) where T : Object
        {
            path = path.ToLower();
            if (null == assetBundleDepend || !assetBundleDepend.ContainsKey(path)) return default(T);

            T data = default(T);
            var bundleName = assetBundleDepend[path];
            if (AssetBundleInfoData.ContainsKey(bundleName))
            {
                data = AssetBundleInfoData[bundleName].LoadAsset<T>(path);
            }

            return data;
        }

        /// <summary>
        /// 解析json数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseDepend(string value)
        {
            assetVersion = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionInfo>(value);

            if (null == assetBundleDepend) assetBundleDepend = new Dictionary<string, string>();
            assetBundleDepend.Clear();

            foreach (var item in assetVersion.dDependInfo)
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
            Maifest = value;
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
