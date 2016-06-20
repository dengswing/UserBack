using UnityEngine;
using System.Collections;
using com.shinezone.network;

public class Demo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        TcpNetwork.Instance.ConnectServer("172.0.0.1", 8080, ResultBack);

        var data = new ByteBuffer();
        data.WriteString("hello");
        TcpNetwork.Instance.SendMessage(data);
    }

    void ResultBack(int code, ByteBuffer data)
    {
        Debug.LogFormat("code =>{0} data=>{1}", code, data);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
