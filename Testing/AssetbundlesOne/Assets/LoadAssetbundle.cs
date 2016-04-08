using UnityEngine;
using System.Collections;
using System.IO;
using Networks.log;

public class LoadAssetbundle : MonoBehaviour
{

    void OnGUI()
    {
        if (GUILayout.Button("LoadAssetbundle"))
        {
            ////首先加载Manifest文件;
            //AssetBundle manifestBundle = AssetBundle.CreateFromFile(Application.dataPath
            //                                                      + "/../Assetbundle/Assetbundle");
            //if (manifestBundle != null)
            //{
            //    AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

            //    //获取依赖文件列表;
            //    string[] cubedepends = manifest.GetAllDependencies("assets/myresources/cube0.prefab");
            //    AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];

            //    for (int index = 0; index < cubedepends.Length; index++)
            //    {
            //        //加载所有的依赖文件;
            //        dependsAssetbundle[index] = AssetBundle.CreateFromFile(Application.dataPath
            //                                                             + "/../Assetbundle/"
            //                                                             + cubedepends[index]);


            //    }

            //    //加载我们需要的文件;"
            //    AssetBundle cubeBundle = AssetBundle.CreateFromFile(Application.dataPath
            //                                                      + "/../Assetbundle/assets/myresources/cube0.prefab");
            //    GameObject cube = cubeBundle.LoadAsset("Cube0") as GameObject;
            //    if (cube != null)
            //    {
            //        Instantiate(cube);
            //    }
            //}

             StartCoroutine("loaderTest");

           

            // StartCoroutine(loader());

        }


    }

    IEnumerator loaderTest()
    {
        string PathURL =
#if UNITY_ANDROID
            "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		    Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        Application.dataPath +"/StreamingAssets";
#else
            string.Empty;
#endif

        var golbalPath = Path.Combine(PathURL, "Windows");
        
        DebugConsole.Instance.Log("0=>" + golbalPath);


        WWW www = new WWW(golbalPath + "/Windows");

        DebugConsole.Instance.Log("0.1=>" + golbalPath + "/Windows");
        yield return www;

        AssetBundle manifestBundle = www.assetBundle;

        DebugConsole.Instance.Log("1=>" + manifestBundle);

        //if (manifestBundle != null)
        //{
        //    AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

        //    DebugConsole.Instance.Log("2=>" + manifest);

        //    //获取依赖文件列表;
        //    string[] cubedepends = manifest.GetAllDependencies("cube.unity3d");

        //    DebugConsole.Instance.Log("3=>" + cubedepends.Length);

        //    AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];

        //    for (int index = 0; index < cubedepends.Length; index++)
        //    {
        //        //加载所有的依赖文件;

        //        dependsAssetbundle[index] = AssetBundle.CreateFromFile(golbalPath + "/" + cubedepends[index]);

        //        DebugConsole.Instance.Log("4=>" + golbalPath + "/" + cubedepends[index]);
        //    }

        //    //加载我们需要的文件;"
        //    AssetBundle cubeBundle = AssetBundle.CreateFromFile(golbalPath + "/cube.unity3d");

        //    DebugConsole.Instance.Log("5=>" + golbalPath + "/cube.unity3d");

        //    GameObject cube = cubeBundle.LoadAsset("Cube") as GameObject;
        //    if (cube != null)
        //    {
        //        DebugConsole.Instance.Log("6=>ok");
        //        Instantiate(cube);
        //    }
        //}
    }


    IEnumerator loader()
    {

        //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
        string PathURL =
#if UNITY_ANDROID
            "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		    Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        "file://" + Application.streamingAssetsPath;
#else
            string.Empty;
#endif

        var assetBundlesPath = Path.Combine(PathURL, "test.txt");

        if (!File.Exists(assetBundlesPath))
        {
            DebugConsole.Instance.Log(assetBundlesPath + "is not exists");
        }

        DebugConsole.Instance.Log(assetBundlesPath);

        // WWW www = WWW.LoadFromCacheOrDownload(assetBundlesPath, 1);

        WWW www = new WWW(assetBundlesPath);
        yield return www;

        if (www.error != null)
        {
            DebugConsole.Instance.Log("Error = " + www.error);
            yield break;
        }

        AssetBundle cubeBundle = null;
        try
        {
            cubeBundle = www.assetBundle;
            DebugConsole.Instance.Log("=11=>" + www.assetBundle);
        }
        catch (System.Exception)
        {
            DebugConsole.Instance.Log("=55=>" + www.text);
        }


        GameObject cube = null;
        try
        {
            cube = cubeBundle.LoadAsset("Cube") as GameObject;
        }
        catch (System.Exception)
        {
            DebugConsole.Instance.Log("=22=>" + www.text);
        }


        DebugConsole.Instance.Log("=44=>");
        if (cube != null)
        {
            DebugConsole.Instance.Log("=33=>");
            Instantiate(cube);
        }
    }

}
