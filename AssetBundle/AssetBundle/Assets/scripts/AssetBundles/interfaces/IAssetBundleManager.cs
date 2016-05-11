using System;

namespace AssetBundles
{
    interface IAssetBundleManager
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">加载的文件路径</param>
        /// <param name="finishBack">加载完成回调</param>
        /// <param name="version">文件的版本号</param>
        void LoadAssetBundle(string path, Action finishBack, int version = 1);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="finishBack">加载完成回调</param>
        /// <param name="version">文件的版本号</param>
        void LoadAssetBundle(Action finishBack, int version = 1);

        /// <summary>
        /// 服务器地址
        /// </summary>
        string ServerUrl { get; set; }
    }
}
