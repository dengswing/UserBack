using AssetBundles.data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AssetBundles
{
    /// <summary>
    /// 把Resource下的资源打包成.unity3d 到StreamingAssets目录下
    /// </summary>
    public class Builder : Editor
    {
        static string sourcePath = Application.dataPath + "/Resources";
        const string assetBundlesOutputPath = "Assets/StreamingAssets"; //打包的位置

        [MenuItem("AssetBundle/Build_Windows")]
        public static void BuildAssetBundleWindows()
        {
            BuildAssetBundle(BuildTarget.StandaloneWindows);
        }

        [MenuItem("AssetBundle/Build_Android")]
        public static void BuildAssetBundleAndroid()
        {
            BuildAssetBundle(BuildTarget.Android);
        }

        [MenuItem("AssetBundle/Build_IOS")]
        public static void BuildAssetBundleIOS()
        {
            BuildAssetBundle(BuildTarget.iOS);
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="buildTarget"></param>
        static void BuildAssetBundle(BuildTarget buildTarget)
        {
            Caching.CleanCache();

            var platform = PathGlobal.GetPlatformFolder(buildTarget);
            string outputPath = Path.Combine(assetBundlesOutputPath, platform);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            Dictionary<string, DependInfo> dInfo = new Dictionary<string, DependInfo>();
            PackManager(sourcePath, dInfo);

            //根据BuildSetting里面所激活的平台进行打包
            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

            VersionManager(assetBundleManifest, dInfo);
            var path = Path.Combine(outputPath, PathGlobal.DEPEND_FILE);
            WriteFile(dInfo, path);

            Debug.LogFormat("{0}打包完成：{1}", platform, dInfo.Count);

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="dInfo"></param>
        static void VersionManager(AssetBundleManifest manifest, Dictionary<string, DependInfo> dInfo)
        {
            var assetBundles = new string[0] { };
            if (null != manifest)
            {
                assetBundles = manifest.GetAllAssetBundles();
            }

            foreach (var item in assetBundles)
            {
                if (!dInfo.ContainsKey(item)) continue;
                dInfo[item].version = manifest.GetAssetBundleHash(item).ToString();
            }
        }

        /// <summary>
        /// 存关系表
        /// </summary>
        /// <param name="info"></param>
        /// <param name="path"></param>
        static void WriteFile(Dictionary<string, DependInfo> dInfo, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var sVersion = JsonConvert.SerializeObject(dInfo);
                byte[] data = Encoding.UTF8.GetBytes(sVersion);
                stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="source"></param>
        static void PackManager(string source, Dictionary<string, DependInfo> dInfo)
        {
            var mdList = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories)
               .Where(file => !file.ToLower().EndsWith(".meta"))
               .ToList();

            foreach (var item in mdList)
            {
                var name = Path.GetFileNameWithoutExtension(item);
                FileAssetBundle(item, dInfo);
            }
        }

        /// <summary>
        /// 找出绑定了bundlename的文件
        /// </summary>
        /// <param name="source"></param>
        static void FileAssetBundle(string source, Dictionary<string, DependInfo> dInfo)
        {
            var path = ReplacePath(source);
            var resource = path.Substring(Application.dataPath.Length + 1);
            var assetPath = string.Format("Assets/{0}", resource);
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);

            var bundleName = assetImporter.assetBundleName;
            if (string.IsNullOrEmpty(bundleName)) return;

            if (bundleName.IndexOf(PathGlobal.BUNDLE_SUFFIX) == -1)
            { //没加后缀的自动加上
                assetImporter.assetBundleName = string.Format("{0}{1}", bundleName, PathGlobal.BUNDLE_SUFFIX);
                bundleName = assetImporter.assetBundleName;
            }

            DependInfo info;
            if (dInfo.ContainsKey(bundleName))
            {
                info = dInfo[bundleName];
            }
            else
            {
                info = new DependInfo();
                dInfo[bundleName] = info;
                info.binding = new List<string>();
                info.bundleName = bundleName;
            }
            var assetName = resource.Substring(resource.IndexOf("/") + 1);
            info.binding.Add(assetName);
        }

        static string ReplacePath(string s)
        {
            return s.Replace("\\", "/");
        }
    }
}
