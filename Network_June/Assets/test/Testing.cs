using UnityEngine;
using System.Collections;
using com.shinezone.network;

public class Testing : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TcpNetwork.Instance.Post();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
