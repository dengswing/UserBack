using System;
namespace AssetBundles
{
    interface IAssetBundleManager
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        string ServerUrl { get; set; }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="finishBack">加载所有资源完成回调</param>
        /// <param name="isHaveUpdate">是否有更新</param>
        void LoadAssetBundle(Action finishBack, bool isHaveUpdate = false);

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        UnityEngine.Object LoadAsset(string name);

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns></returns>
        T LoadAsset<T>(string name) where T : UnityEngine.Object;
    }
}
