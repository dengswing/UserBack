using UnityEngine;
using System.Collections;
using com.shinezone.network;

public class Demo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // UdpNetwork.Instance.ConnectServer("127.0.0.1", 8080, ResultBack);

        // var data = new ByteBuffer();
        // data.WriteString("hello");
        // UdpNetwork.Instance.SendMessage(data);


        // BaseThread.GetInstance();

        // var url = "http://dev-mi-facebook.shinezone.com/index.php?*=[[\"game.login\",[\"1\",0,0,1,\"3\",\"4\"]]]&halt=711&sign=6YjG2QUodzBRd9RDQweiig2s3MQ%3D";
        // HttpNetwork.Instance.SendMessage(url, HttpResultBack);



        TcpNetwork.Instance.ConnectServer("127.0.0.1", 8080, ResultBack);

        var data = new ByteBuffer();
        data.WriteString("hello");
        TcpNetwork.Instance.SendMessage(data);
    }

    void ResultBack(int code, ByteBuffer data)
    {
        Debug.LogFormat("code =>{0} data=>{1}", code, data);
    }

    void HttpResultBack(int code, object data)
    {
        Debug.LogFormat("http code =>{0} data=>{1}", code, data);
    }



    void OnDestroy()
    {
        UdpNetwork.Instance.Shutdown();
    }
}
