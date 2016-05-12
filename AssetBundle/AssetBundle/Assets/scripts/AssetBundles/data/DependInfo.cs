using System.Collections.Generic;

namespace AssetBundles.data
{
    /// <summary>
    /// 依赖信息
    /// </summary>
    public class DependInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string version;

        /// <summary>
        /// 唯一名称
        /// </summary>
         public string bundleName;

        /// <summary>
        /// 绑定路径
        /// </summary>
        public List<string> binding;
    }

    public class VersionInfo
    {
        /// <summary>
        /// 主版本号
        /// </summary>
        public int maifestVersion;

        /// <summary>
        /// 所有依赖
        /// </summary>
        public Dictionary<string, DependInfo> dDependInfo;
    }
}
