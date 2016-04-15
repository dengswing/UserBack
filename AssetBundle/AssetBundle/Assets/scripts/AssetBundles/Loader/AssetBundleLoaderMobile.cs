using System.Collections;
using System.IO;
using UnityEngine;

namespace AssetBundles.Loader
{
    public class AssetBundleLoaderMobile : AssetBundleLoaderAbs
    {
        protected AssetBundle bundle;

        public override void Load()
        {
            base.Load();

            if (state == LoadState.State_None)
            {
                state = LoadState.State_Loading;
                bundleManager.RequestLoadBundle(this);
            }
            else if (state == LoadState.State_Error)
            {
                Error();
            }
            else if (state == LoadState.State_Complete)
            {
                Complete();
            }
        }

        public override void LoadBundle()
        {
            base.LoadBundle();

            var platfrom = PathGlobal.GetPlatformFile();
            if (bundleData != null) bundleName = bundleData.bundleName;
            var serverPath = PathGlobal.GetStreamingAssetsSourceFile(platfrom);
            var localPath = PathGlobal.GetPersistentDataPathSourceFile(platfrom);

#if DEBUG_CONSOLE
            Networks.log.DebugConsole.Instance.Log("LoadBundle:: serverPath=" + serverPath + "|localPath=" + localPath);
#endif

            if (File.Exists(localPath))
                bundleManager.StartCoroutine(LocalLoder(localPath, bundleName));
            else
                bundleManager.StartCoroutine(ServerLoader(serverPath, bundleName));
        }

        protected virtual IEnumerator LocalLoder(string path, string bundleName)
        {
            //AssetBundleCreateRequest load = AssetBundle.LoadFromFileAsync(path);
            //yield return load;
            //AssetBundle manifestBundle = load.assetBundle;

            //if (manifestBundle != null)
            //{
            //    var sAssetName = string.Format("{0}.unity3d", bundleName);

            //    //加载依赖
            //    AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
            //    string[] cubedepends = manifest.GetAllDependencies(sAssetName);
            //    AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];
            //    for (int index = 0; index < cubedepends.Length; index++)
            //    {
            //        load = AssetBundle.LoadFromFileAsync(PathGlobal.GetJoinPath(path, cubedepends[index]));
            //        yield return load;
            //        dependsAssetbundle[index] = load.assetBundle;
            //    }

            var sAssetName = string.Format("{0}.unity3d", bundleName);
            AssetBundleCreateRequest load = AssetBundle.LoadFromFileAsync(PathGlobal.GetJoinPath(path, sAssetName));

#if DEBUG_CONSOLE
            Networks.log.DebugConsole.Instance.Log("LocalLoder:: url=" + PathGlobal.GetJoinPath(path, sAssetName));
#endif
            yield return load;
            bundle = load.assetBundle;
            Complete();
            //}
        }


        protected virtual IEnumerator ServerLoader(string path, string bundleName)
        {
            //var platfrom = PathGlobal.GetPlatformFile();
            //WWW www = new WWW(path);
            //yield return www;
            //if (www.error != null)
            //{
            //    Error();
            //    yield return null;
            //}

            //AssetBundle manifestBundle = www.assetBundle;

            //ReplaceLocalRes(bundleName, www);

            //if (manifestBundle != null)
            //{
            //    var sAssetName = string.Format("{0}.unity3d", bundleName);

            //    //加载依赖
            //    AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
            //    string[] cubedepends = manifest.GetAllDependencies(sAssetName);
            //    AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];
            //    for (int index = 0; index < cubedepends.Length; index++)
            //    {
            //        www = new WWW(PathGlobal.GetJoinPath(path, cubedepends[index]));
            //        yield return www;
            //        dependsAssetbundle[index] = www.assetBundle;
            //        ReplaceLocalRes(cubedepends[index], www);
            //    }

            var sAssetName = string.Format("{0}.unity3d", bundleName);
            WWW www = new WWW(PathGlobal.GetJoinPath(path, sAssetName));

#if DEBUG_CONSOLE
            Networks.log.DebugConsole.Instance.Log("ServerLoader:: url=" + www.url);
#endif
            yield return www;
            if (www.error != null)
            {
                Error();
                yield return null;
            }
            bundle = www.assetBundle;
            ReplaceLocalRes(sAssetName, www);
            Complete();
            www = null;
            // }
        }

        void ReplaceLocalRes(string fileName, WWW www)
        {
            if (www == null) return;
            var path = PathGlobal.GetJoinPath(PathGlobal.GetPlatformFile(), fileName);
            path = PathGlobal.GetPersistentDataPathSourceFile(path);

            //if (!Directory.Exists(path))
            //{
            //    // Create the directory it does not exist.
            //    Directory.CreateDirectory(path);
            //}

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {

#if DEBUG_CONSOLE
                Networks.log.DebugConsole.Instance.Log("ReplaceLocalRes:: url=" + path);
#endif
                byte[] data = www.bytes;
                stream.Write(data, 0, data.Length);

#if DEBUG_CONSOLE
                Networks.log.DebugConsole.Instance.Log("ReplaceLocalRes:: write success");
#endif
                www.Dispose();
                www = null;
            }
        }

        protected override void Complete()
        {
            state = LoadState.State_Complete;
            if (bundle != null) CreateBundleInfo(bundle);
            bundle = null;
            base.Complete();
        }

        protected override void Error()
        {
            state = LoadState.State_Error;
            base.Error();
        }
    }
}