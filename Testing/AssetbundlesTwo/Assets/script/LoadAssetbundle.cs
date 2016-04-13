using Networks.log;
using System.Collections;
using System.IO;
using UnityEngine;

public class LoadAssetbundle : MonoBehaviour
{

    void Awake()
    {
        StartCoroutine("loaderTest");

        //var ss = AssetDatabase.LoadMainAssetAtPath("Assets/res/Materials/20121222171635.mat") as Material;
        //ss.mainTexture = AssetDatabase.LoadMainAssetAtPath("Assets/res/Q20160411112059.png") as Texture;
       // cube.GetComponent<Renderer>().material.mainTexture = AssetDatabase.LoadMainAssetAtPath("Assets/res/Q20160411112059.png") as Texture;
    }

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
        var buildTarget = Platform.GetPlatformFile();
        var golbalPath = Platform.GetStreamingAssetsSourceFile(buildTarget);
        DebugConsole.Instance.Log("0=>" + golbalPath);
        //AssetBundle manifestBundle = AssetBundle.CreateFromFile(golbalPath + "/" + buildTarget);
        var version = 102;
        WWW www = new WWW(golbalPath + "/" + buildTarget);
        DebugConsole.Instance.Log("2=>" + www.url);
        yield return www;
        AssetBundle manifestBundle = www.assetBundle;
        DebugConsole.Instance.Log("1.1=>" + manifestBundle);
        if (manifestBundle != null)
        {
            AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

            DebugConsole.Instance.Log("2=>" + manifest);

            var sAssetName = "cylinder.unity3d";


            //获取依赖文件列表;
            string[] cubedepends = manifest.GetAllDependencies(sAssetName);

            DebugConsole.Instance.Log("3=>" + cubedepends.Length);

            AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];

            for (int index = 0; index < cubedepends.Length; index++)
            {
                //加载所有的依赖文件;

                //golbalPath = Platform.GetStreamingAssetsSourceFile(buildTarget,false);

                www = new WWW(golbalPath + "/" + cubedepends[index]);
                //DebugConsole.Instance.Log("1=>" + manifestBundle);
                Debug.Log(www.url);
                yield return www;
                try
                {
                    dependsAssetbundle[index] = www.assetBundle;
                }
                catch (System.Exception)
                {
                    Debug.Log(www.url);
                }
                
                //dependsAssetbundle[index] = AssetBundle.CreateFromFile(golbalPath + "/" + cubedepends[index]);

                DebugConsole.Instance.Log("4=>" + www.url);
            }

            //加载我们需要的文件;"
            www = new WWW(golbalPath + "/"+ sAssetName);
            DebugConsole.Instance.Log("5=>" + www.url);
            yield return www;
            AssetBundle cubeBundle = www.assetBundle;

            
            DebugConsole.Instance.Log("7=>" + cubeBundle);

            GameObject cube = null;
            try
            {
                cube = cubeBundle.LoadAsset("Cylinder") as GameObject;
            }
            catch (System.Exception)
            {
                DebugConsole.Instance.Log("7.1=>" + " ...");
            }
            
            DebugConsole.Instance.Log("7.2=>" + cube+" ...");
            if (cube != null)
            {
                DebugConsole.Instance.Log("8=>ok");
                Instantiate(cube);
            }

            cube = cubeBundle.LoadAsset("why") as GameObject;
            if (cube != null)
            {
                DebugConsole.Instance.Log("8.1=>ok");
                Instantiate(cube);
            }
            

            GameObject Sphere = cubeBundle.LoadAsset("CubeNo") as GameObject;
            if (Sphere != null)
            {
                DebugConsole.Instance.Log("9=>ok");
                Sphere = Instantiate(Sphere);
                Sphere.transform.position = new Vector3(2, 0, 0);
                //Sphere.transform.Rotate(new Vector3(180, 0, 0));
                //Sphere.transform.localScale = new Vector3(2, 2, 2);
            }


            cube = GameObject.Find("Cube");
            cube.GetComponent<Renderer>().material.mainTexture = cubeBundle.LoadAsset("Q20160411112059") as Texture;

            var all = cubeBundle.GetAllAssetNames();
            foreach(var i in all) {
                Debug.Log(i);
            }
        }
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
