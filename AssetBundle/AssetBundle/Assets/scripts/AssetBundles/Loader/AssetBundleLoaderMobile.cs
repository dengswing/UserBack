using System.Collections;
using System.IO;
using UnityEngine;

namespace AssetBundles.Loader
{
    public class AssetBundleLoaderMobile : AssetBundleLoaderAbs
    {
        protected AssetBundle bundle;

        /// <summary>
        /// 开始加载
        /// </summary>
        public override void LoadBundle()
        {
            base.LoadBundle();

            if (state == LoadState.State_None)
            {
                state = LoadState.State_Loading;
                LoaderBundle();
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

        void LoaderBundle()
        {
            var platfrom = PathGlobal.GetPlatformFile();
            var path = PathGlobal.GetStreamingAssetsSourceFile(platfrom);
            path = PathGlobal.GetJoinPath(path, bundleData.bundleName);
            Hash128 version = bundleManager.GetBundleHash(bundleData.bundleName);
            bundleManager.LoaderManager((AssetBundle asset) =>
            {
                bundle = asset;
                Complete();
            }, path, version);
        }

        #region loader version old

        //        void LoaderBundle()
        //        {
        //            var platfrom = PathGlobal.GetPlatformFile();
        //            if (bundleData != null) bundleName = bundleData.bundleName;
        //            var serverPath = PathGlobal.GetStreamingAssetsSourceFile(platfrom);
        //            var localPath = PathGlobal.GetPersistentDataPathSourceFile(platfrom);

        //#if DEBUG_CONSOLE
        //            UnityEngine.Debug.Log("LoadBundle:: serverPath=" + serverPath + "|localPath=" + localPath);
        //#endif

        //            var sAssetName = string.Format("{0}.unity3d", bundleName);
        //            sAssetName = PathGlobal.GetJoinPath(localPath, sAssetName);

        //#if DEBUG_CONSOLE
        //            UnityEngine.Debug.Log("LoadBundle:: sAssetName=" + sAssetName);
        //#endif
        //            if (File.Exists(sAssetName))
        //                bundleManager.StartCoroutine(LocalLoder(localPath, bundleName));
        //            else
        //                bundleManager.StartCoroutine(ServerLoader(serverPath, bundleName));
        //        }

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
            UnityEngine.Debug.Log("LocalLoder:: url=" + PathGlobal.GetJoinPath(path, sAssetName));
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
            UnityEngine.Debug.Log("ServerLoader:: url=" + www.url);
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

            var deposit = path.Substring(0, path.LastIndexOf("/"));

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("ReplaceLocalRes:: deposit=" + deposit);
#endif
            if (!Directory.Exists(deposit))
            {
                Directory.CreateDirectory(deposit);
            }


#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("ReplaceLocalRes:: path=" + path);
#endif
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {

#if DEBUG_CONSOLE
                UnityEngine.Debug.Log("ReplaceLocalRes:: url=" + path);
#endif
                byte[] data = www.bytes;
                stream.Write(data, 0, data.Length);

#if DEBUG_CONSOLE
                UnityEngine.Debug.Log("ReplaceLocalRes:: write success");
#endif
                www.Dispose();
                www = null;
            }
        }

        #endregion

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