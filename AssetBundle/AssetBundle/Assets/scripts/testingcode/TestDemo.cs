
using AssetBundles;
using AssetBundles.Loader;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            assetBundleManager.LoadAssetBundleAsync<GameObject>("cube.prefab", (GameObject cube) =>
            {
                var tmp = Instantiate(cube);
                tmp.transform.position = new Vector3(4, 0, 0);
            });

            assetBundleManager.LoadAssetBundleAsync<GameObject>("cube.prefab", (GameObject cube) =>
            {
                var tmp = Instantiate(cube);
                tmp.transform.position = new Vector3(-4, 0, 0);
            });

            assetBundleManager.LoadAssetBundleAsync<GameObject>("cube.prefab", (GameObject cube) =>
            {
                var tmp = Instantiate(cube);
                tmp.transform.position = new Vector3(0, 4, 0);
            });

            assetBundleManager.LoadAssetBundleAsync<GameObject>("cube.prefab", (GameObject cube) =>
             {
                 Instantiate(cube);
             });

            assetBundleManager.LoadAssetBundleAsync<Texture>("aa/cube.png", (Texture t) =>
            {
                Cube.GetComponent<MeshRenderer>().materials[0].mainTexture = t;
            });

            assetBundleManager.LoadAssetBundleAsync<Material>("aa/Materials/pic_1.mat", (Material m) =>
            {
                Debug.Log("get " + m);
            });





            //Debug.Log("load init finish");
            // var cube = assetBundleManager.GetAsset<GameObject>("cube.prefab");
            //var dd = assetBundleManager.LoadAssetBundleAsync<Texture>("aa/cube.png");
            //Debug.Log("get "+ cube+"|"+sss+"|"+dd);

            //   var unity = assetBundleManager.GetAsset("one.unity");

            //SceneManager.LoadScene();

            //  Debug.Log("get " + cube + "|" + sss + "|" + dd);

        }, true);
    }
}
