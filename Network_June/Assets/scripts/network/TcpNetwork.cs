using System;
using System.IO;
using System.Net.Sockets

namespace com.shinezone.network
{
    public class TcpNetwork : Network<TcpNetwork>
    {
        private TcpClient client = null;
        private NetworkStream outStream = null;
        private MemoryStream memStream;
        private BinaryReader reader;

        private const int MAX_READ = 8192;
        private byte[] byteBuffer = new byte[MAX_READ];
        public static bool loggedIn = false;

        protected override void Init()
        {
            base.Init();

            memStream = new MemoryStream();
            reader = new BinaryReader(memStream);
        }

        public void ConnectServer(string host, int port)
        {

        }
        
        private void ConnectClient(string host, int port)
        {
            client = null;
            client = new TcpClient();
            client.SendTimeout = 1000;
            client.ReceiveTimeout = 1000;
            client.NoDelay = true;
            try
            {
                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
            }
            catch (Exception e)
            {
                Close(); UnityEngine.Debug.LogError(e.Message);
            }
        }

        public void Close()
        {
            if (client != null)
            {
                if (client.Connected) client.Close();
                client = null;
            }
        }
        
        void OnConnect(IAsyncResult asr)
        {
            outStream = client.GetStream();
            client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
            
            //todu
        }

        void OnRead(IAsyncResult asr)
        {
            int bytesRead = 0;
            try
            {
                lock (client.GetStream())
                {         //读取字节流到缓冲区
                    bytesRead = client.GetStream().EndRead(asr);
                }
                if (bytesRead < 1)
                {                //包尺寸有问题，断线处理
                    OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                    return;
                }
                OnReceive(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
                lock (client.GetStream())
                {         //分析完，再次监听服务器发过来的新消息
                    Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
                    client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
                }
            }
            catch (Exception ex)
            {
                //PrintBytes();
                OnDisconnected(DisType.Exception, ex.Message);
            }
        }

        void OnDisconnected(DisType dis, string msg)
        {
            Close();   //关掉客户端链接
            int protocal = dis == DisType.Exception ?
            Protocal.Exception : Protocal.Disconnect;

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteShort((ushort)protocal);

            UnityEngine.Debug.LogError("Connection was closed by the server:>" + msg + " Distype:>" + dis);
        }

        protected override void Send()
        {
            base.Send();
        }

        protected override void Recv()
        {
            base.Recv();
        }

    }
}
