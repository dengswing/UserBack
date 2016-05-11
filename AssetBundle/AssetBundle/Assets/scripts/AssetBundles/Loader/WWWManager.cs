using System.Collections;
using System.IO;
using UnityEngine;

namespace AssetBundles.Loader
{
    public class WWWManager
    {

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="path"></param>
        public void LoaderFile(string path)
        {

        }



        protected virtual IEnumerator LocalLoder(string path, string bundleName)
        {
            var sAssetName = string.Format("{0}.unity3d", bundleName);
            AssetBundleCreateRequest load = AssetBundle.LoadFromFileAsync(PathGlobal.GetJoinPath(path, sAssetName));

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("LocalLoder:: url=" + PathGlobal.GetJoinPath(path, sAssetName));
#endif
            yield return load;
        }


        IEnumerator ServerLoader(string path)
        {
            WWW www = new WWW(path);

#if DEBUG_CONSOLE
            UnityEngine.Debug.Log("ServerLoader:: url=" + www.url);
#endif
            yield return www;
            if (www.error != null)
            {
                yield return null;
            }
            ReplaceLocalRes(www);
            www = null;
        }

        void ReplaceLocalRes(WWW www)
        {
            if (www == null || www.bytes == null) return;
            var path = www.url;
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

#if 

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

    }
}