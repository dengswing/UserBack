using AssetBundles;
using UnityEngine;

/// <summary>
/// 每个bundle的信息
/// </summary>
public class TestDemo : MonoBehaviour
{
    [SerializeField]
    public TEST_STATE testState;

    public enum TEST_STATE
    {
        LOAD_ASSET,
        LOAD_ASSET_ASYNC
    }

    void Start()
    {
        var assetBundleManager = AssetBundleManager.Instance;

        var Cube = GameObject.Find("Cube");
        assetBundleManager.LoadAssetBundle(() =>
        {
            Debug.Log("Init Finish");
            if (testState == TEST_STATE.LOAD_ASSET)
                LoadAssetBundle(assetBundleManager);
            else
                LoadAssetBundleAsync(assetBundleManager);

        }, true);
    }

    void LoadAssetBundleAsync(AssetBundleManager assetBundleManager)
    {
        assetBundleManager.LoadAssetBundleAsync<GameObject>("cube.prefab", (GameObject cube) =>
        {
            var tmp = Instantiate(cube);
            tmp.transform.position = new Vector3(4, 0, 0);
        });

        var Cube = GameObject.Find("Cube");
        assetBundleManager.LoadAssetBundleAsync<Texture>("aa/cube.png", (Texture t) =>
        {
            Cube.GetComponent<MeshRenderer>().materials[0].mainTexture = t;
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

        assetBundleManager.LoadAssetBundleAsync<Material>("aa/Materials/pic_1.mat", (Material m) =>
        {
            Debug.Log("get " + m);
        });

        assetBundleManager.LoadAssetBundleAsync<GameObject>("cube.prefab", (GameObject cube) =>
        {
            Instantiate(cube);
        });
    }

    void LoadAssetBundle(AssetBundleManager assetBundleManager)
    {
        var cube = assetBundleManager.LoadAsset<GameObject>("cube.prefab");
        var tmp = Instantiate(cube);
        tmp.transform.position = new Vector3(4, 0, 0);

        cube = assetBundleManager.LoadAsset<GameObject>("cube.prefab");
        tmp = Instantiate(cube);
        tmp.transform.position = new Vector3(-4, 0, 0);

        cube = assetBundleManager.LoadAsset<GameObject>("cube.prefab");
        tmp = Instantiate(cube);
        tmp.transform.position = new Vector3(0, 4, 0);

        cube = assetBundleManager.LoadAsset<GameObject>("cube.prefab");
        tmp = Instantiate(cube);

        var Cube = GameObject.Find("Cube");
        var t = assetBundleManager.LoadAsset<Texture>("aa/cube.png");
        Cube.GetComponent<MeshRenderer>().materials[0].mainTexture = t;

        var m = assetBundleManager.LoadAsset<Material>("aa/Materials/pic_1.mat");
        Debug.Log("get " + m);
    }
}
