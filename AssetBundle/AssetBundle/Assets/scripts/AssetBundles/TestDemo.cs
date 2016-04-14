
using AssetBundles;
using AssetBundles.Loader;
using UnityEngine;

/// <summary>
/// 每个bundle的信息
/// </summary>
public class TestDeme : MonoBehaviour
{
    void Start() 
    {
        var assetBundleManager = new AssetBundleManager();

        assetBundleManager.loaderComplete = CallBackLoaderComplete;
        assetBundleManager.CreateLoader("ddd");

    }

    void CallBackLoaderComplete(AssetBundleInfo info) 
    {
        Instantiate(info.LoadAsset("cube"));
    }
}
