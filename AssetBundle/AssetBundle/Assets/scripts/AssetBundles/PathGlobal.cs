#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace AssetBundles
{
    public class PathGlobal
    {

        /// <summary>
        /// 平台文件夹
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 平台文件夹
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Application.streamingAssetsPath
        /// </summary>
        /// <param name="path"></param>
        /// <param name="forWWW"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Application.persistentDataPath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPersistentDataPathSourceFile(string path)
        {

#if UNITY_EDITOR
            return GetJoinPath(Application.persistentDataPath, path);
#else
            return GetJoinPath(Application.persistentDataPath, path);
#endif


        }

        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetJoinPath(string path, string name)
        {
            return string.Format("{0}/{1}", path, name);
        }
    }
}