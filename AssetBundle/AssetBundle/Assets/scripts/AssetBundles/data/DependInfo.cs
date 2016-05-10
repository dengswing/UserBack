using System.Collections.Generic;

namespace AssetBundles.data
{
    /// <summary>
    /// 解析数据管理
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
        // public string bundleName;

        /// <summary>
        /// 绑定路径
        /// </summary>
        public List<string> binding;
    }
}
