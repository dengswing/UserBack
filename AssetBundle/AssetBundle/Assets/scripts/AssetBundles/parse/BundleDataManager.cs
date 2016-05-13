using AssetBundles.data;
using AssetBundles.Loader;
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

        AssetBundleManager bundleManager;

        QueueLoaderAsset queueLoaderAsset;

        EditorInfo assetInfo;

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

        public BundleDataManager(AssetBundleManager bundleManager)
        {
            this.bundleManager = bundleManager;
            AssetBundleInfoData = new Dictionary<string, AssetBundleInfo>();
            queueLoaderAsset = new QueueLoaderAsset(bundleManager);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public T LoadAsset<T>(string name) where T : Object
        {
            return LoadAssetBundle<T>(name);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="finishBack"></param>
        /// <returns></returns>
        public T LoadAssetBundleAsync<T>(string name, CallBackAssetComplete<T> finishBack) where T : Object
        {
            return LoadAssetBundle<T>(name, finishBack, true);
        }

        #region load asset
        T LoadAssetBundle<T>(string path, CallBackAssetComplete<T> finishBack = null, bool isAsync = false) where T : Object
        {
            path = path.ToLower();
            T data = default(T);

            if (null == assetBundleDepend || !assetBundleDepend.ContainsKey(path))
            {
                data = GetLocalAsset<T>(path);
                if (isAsync && finishBack != null) finishBack(data);
                return data;
            }
            
            var bundleName = assetBundleDepend[path];
            if (AssetBundleInfoData.ContainsKey(bundleName))
            {
//#if UNITY_EDITOR
//                data = AssetBundleInfoData[bundleName].LoadAsset<T>(path);
//                if (isAsync && finishBack != null) finishBack(data);
//#else
                if (isAsync)
                    LoadAssetBundleAsync<T>(AssetBundleInfoData[bundleName], path, finishBack);
                else
                    data = AssetBundleInfoData[bundleName].LoadAsset<T>(path);
//#endif
            }
            return data;
        }

        void LoadAssetBundleAsync<T>(AssetBundleInfo info, string path, CallBackAssetComplete<T> finishBack) where T : Object
        {
            queueLoaderAsset.LoadAssetBundleAsync(info, path, (Object asset) =>
            {
                if (null != finishBack) finishBack((T)asset);
            });
        }

        T GetLocalAsset<T>(string path) where T : Object
        {
            if (null == assetInfo) assetInfo = new EditorInfo();
            return assetInfo.LoadAsset<T>(path);
        }

        #endregion

        /// <summary>
        /// 解析json数据
        /// </summary>
        /// <param name="value"></param>
        internal void ParseDepend(string value)
        {
            assetVersion = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionInfo>(value);

            if (null == assetVersion) return;

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
