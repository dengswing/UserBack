using UnityEngine;
namespace AssetBundles
{  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundle"></param>
    public delegate void CallBackAssetBundle(AssetBundle bundle);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    public delegate void CallBackLoaderComplete(AssetBundleInfo info);
}
