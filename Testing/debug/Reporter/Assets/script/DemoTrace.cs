using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DemoTrace : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("sstestsa");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //[MenuItem("GameObject/UI/Image")]
    //static void CreateText()
    //{
    //    if (Selection.activeTransform)
    //    {
    //        if (Selection.activeTransform.GetComponentInParent<Canvas>())
    //        {
    //            var go = new GameObject("image", typeof(Image));
    //            go.GetComponent<Image>().raycastTarget = false;
    //            go.transform.SetParent(Selection.activeTransform);
    //        }
    //    }
    //}
}
