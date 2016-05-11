using System;

namespace AssetBundles
{
    interface IAssetBundleManager
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="finishBack">加载所有资源完成回调</param>
        /// <param name="isHaveUpdate">是否有更新</param>
        void LoadAssetBundle(Action finishBack, bool isHaveUpdate = false);

        /// <summary>
        /// 服务器地址
        /// </summary>
        string ServerUrl { get; set; }
    }
}
