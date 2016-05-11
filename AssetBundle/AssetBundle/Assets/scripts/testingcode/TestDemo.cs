
using AssetBundles;
using AssetBundles.Loader;
using UnityEngine;

/// <summary>
/// 每个bundle的信息
/// </summary>
public class TestDemo : MonoBehaviour
{
    void Start() 
    {
        var assetBundleManager = AssetBundleManager.Instance;
        //assetBundleManager.Load("AssectFrist",CallBackLoaderComplete);

        assetBundleManager.LoadAssetBundle(() =>
        {
            Debug.Log("load init finish");
        },true);
    }

//    void CallBackLoaderComplete(AssetBundleInfo info) 
//    {
//#if DEBUG_CONSOLE
//        UnityEngine.Debug.Log(info.ToString());
//#endif
//        Instantiate(info.LoadAsset("cube"));
//    }
}
