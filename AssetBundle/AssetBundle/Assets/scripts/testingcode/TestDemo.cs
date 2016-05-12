﻿
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

        var Cube = GameObject.Find("Cube");
        assetBundleManager.LoadAssetBundle(() =>
        {
            Debug.Log("load init finish");
             var cube = assetBundleManager.GetAsset<GameObject>("cube.prefab");
            Instantiate(cube);

            var sss = assetBundleManager.GetAsset<Material>("aa/Materials/pic_1.mat");

            var dd = assetBundleManager.GetAsset<Texture>("aa/cube.png");

            Cube.GetComponent<MeshRenderer>().materials[0].mainTexture = dd;

            Debug.Log("get "+ cube+"|"+sss+"|"+dd);

        }, true);
    }

    //    void CallBackLoaderComplete(AssetBundleInfo info) 
    //    {
    //#if DEBUG_CONSOLE
    //        UnityEngine.Debug.Log(info.ToString());
    //#endif
    //        Instantiate(info.LoadAsset("cube"));
    //    }
}
