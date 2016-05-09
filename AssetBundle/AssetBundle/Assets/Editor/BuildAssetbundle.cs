using AssetBundles;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 把Resource下的资源打包成.unity3d 到StreamingAssets目录下
/// </summary>
public class Builder : Editor
{
    static string sourcePath = Application.dataPath + "/Resource";
    const string AssetBundlesOutputPath = "Assets/StreamingAssets";

    [MenuItem("AssetBundle/Build_Windows")]
    public static void BuildAssetBundle_Windows()
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows);
    }

    [MenuItem("AssetBundle/Build_Android")]
    public static void BuildAssetBundle_Android()
    {
        BuildAssetBundle(BuildTarget.Android);
    }

    [MenuItem("AssetBundle/Build_IOS")]
    public static void BuildAssetBundle_IOS()
    {
        BuildAssetBundle(BuildTarget.iOS);
    }

    static void BuildAssetBundle(BuildTarget buildTarget)
    {
        Caching.CleanCache();       

        Pack(sourcePath);

        string outputPath = Path.Combine(AssetBundlesOutputPath, PathGlobal.GetPlatformFolder(buildTarget));
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        //根据BuildSetting里面所激活的平台进行打包
        var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

        var assetBundles = assetBundleManifest.GetAllAssetBundles();
        var len = assetBundleManifest.GetAllAssetBundlesWithVariant();

        AssetDatabase.Refresh();
        Debug.Log(assetBundleManifest.name + @"打包完成：" + assetBundles.Length);
    }

    static void Pack(string source)
    {
        var mdList = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories)
           .Where(file => !file.ToLower().EndsWith(".meta"))
           .ToList();

        var length = (mdList != null ? mdList.Count : 0);
        for (var i = 0; i < length; i++)
        {
            var name = Path.GetFileNameWithoutExtension(mdList[i]);
            FileAssetBundle(mdList[i]);
        }        
    }

    static void FileAssetBundle(string source)
    {
        var path = ReplacePath(source);
        var resource = path.Substring(Application.dataPath.Length + 1);
        var assetPath = string.Format("Assets/{0}", resource);

        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);

        if (!string.IsNullOrEmpty(assetImporter.assetBundleName))
        {
            var assetName = resource.Substring(resource.IndexOf("/") + 1);
            Debug.Log("||+>" + assetImporter.assetBundleName + "|" + assetName);
        }
    }

    static string ReplacePath(string s)
    {
        return s.Replace("\\", "/");
    }
}
