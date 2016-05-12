using AssetBundles.data;
using AssetBundles.Loader;
namespace AssetBundles
{
    /// <summary>
    /// 加载回调
    /// </summary>
    /// <param name="info"></param>
    public delegate void CallBackLoader(AssetBundleLoaderAbs info);

    /// <summary>
    /// 加载完成
    /// </summary>
    /// <param name="info"></param>
    public delegate void CallBackLoaderComplete(AssetBundleInfo info);

    /// <summary>
    /// 加载资源完成
    /// </summary>
    /// <param name="info"></param>
    public delegate void CallBackAssetComplete<T>(T info) where T : UnityEngine.Object;
}
