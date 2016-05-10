using AssetBundles.data;
using UnityEngine;
namespace AssetBundles
{  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundle"></param>
    public delegate void CallBackAssetBundle(AssetBundle bundle);

    /// <summary>
    /// 加载完成回调
    /// </summary>
    /// <param name="info"></param>
    public delegate void CallBackLoaderComplete(AssetBundleInfo info);
}
