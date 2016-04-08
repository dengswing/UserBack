#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Platform
{

#if UNITY_EDITOR
    public static string GetPlatformFolder(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "IOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            default:
                return null;
        }
    }
#endif

    public static string GetPlatformFile()
    {
#if UNITY_EDITOR
        return "Windows";
#elif UNITY_ANDROID
       return "Android";
#else
       return "IOS"
#endif
    }

    public static string GetStreamingAssetsSourceFile(string path, bool forWWW = true)
    {
        string filePath = null;
#if UNITY_EDITOR
        if (forWWW)
            filePath = string.Format("file://{0}/StreamingAssets/{1}", Application.dataPath, path);
        else
            filePath = string.Format("{0}/StreamingAssets/{1}", Application.dataPath, path);
#elif UNITY_ANDROID
        filePath = string.Format("jar:file://{0}!/assets/{1}", Application.dataPath, path);
#else
        filePath = string.Format("file://{0}/Raw/{1}", Application.dataPath, path);
#endif
        return filePath;
    }
}