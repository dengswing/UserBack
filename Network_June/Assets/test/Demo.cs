using jun;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public Button tcpSend;
    public Button tcpConn;
    public Button udpSend;
    public Button udpConn;
    public Button httpSend;

    // Use this for initialization
    void Start()
    {
        tcpSend.onClick.AddListener(OnClickTcpSend);
        tcpConn.onClick.AddListener(OnClickTcpConn);

        udpSend.onClick.AddListener(OnClickUdpSend);
        udpConn.onClick.AddListener(OnClickUdpConn);
        httpSend.onClick.AddListener(OnClickHttpSend); 
    }

    void OnClickHttpSend()
    {
        HttpNetwork.Instance.RequestPost("http://dev-soul.shinezoneapp.com/?dev=jinfeifei",
                                         "*=[[\"test.data\",[\"9\",0,0]],[\"test.try\",[\"9\",0,0,\"4\"]]]&halt=161&sign=sOxoC37X9eWdNvIyW6igRqOcHoU%3D",
                                         HttpResultBack);
    }

    void OnClickTcpSend()
    {
        var data = new JunByteBuffer();
        data.WriteString("hello tcp");
        TcpNetwork.Instance.SendMessage(data);
    }

    void OnClickTcpConn()
    {
        TcpNetwork.Instance.ConnectServer("127.0.0.1", 8885, ResultBack);
    }

    void OnClickUdpSend()
    {
        var data = new JunByteBuffer();
        data.WriteString("hello udp");
        UdpNetwork.Instance.SendMessage(data);
    }
    void OnClickUdpConn()
    {
        UdpNetwork.Instance.ConnectServer("127.0.0.1", 8001, ResultBack);
    }

    void ResultBack(int code, JunByteBuffer data)
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
