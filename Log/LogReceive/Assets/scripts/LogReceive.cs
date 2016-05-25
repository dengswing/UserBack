using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class LogReceive : MonoBehaviour
{ 
    // Use this for initialization
    void Start()
    {
        InitializeEnvironment();
        //Send();
    }

    void Update()
    {
        // SendMessages("udp udp udp！！！！！！欢迎");
    }

    int port = 8081;          //server listen port,need add to firewall             
    Socket server;
    IPEndPoint ipClient;

    void InitializeEnvironment()
    {
        try
        {
            var _server = new UdpClient(port);
            var _client = new IPEndPoint(IPAddress.Any, port);
            UnityEngine.Debug.Log("Address===" + _client.Address.ToString());
            Thread thread = new Thread(delegate ()
            {
                try
                {
                    while (true)
                    {
                        var _buffer = _server.Receive(ref _client);
                        string log = Encoding.UTF8.GetString(_buffer);
                        UnityEngine.Debug.Log("log===" + log);
                    }
                }
                catch (System.Exception ey)
                {
                    UnityEngine.Debug.Log("ERROR=" + ey);
                }
            });
            thread.Start();

        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.Log("ERROR===" + ex);
        }
    }

    void Send()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        var ip = IPAddress.Parse("172.17.1.238");
        ipClient = new IPEndPoint(ip, port);  //设置服务IP，设置TCP端口号
        SendMessages("test123456");
    }

    void SendMessages(string message)
    {
        byte[] data = new byte[1024];
        data = Encoding.ASCII.GetBytes(message);
        server.SendTo(data, data.Length, SocketFlags.None, ipClient);
    }
}
