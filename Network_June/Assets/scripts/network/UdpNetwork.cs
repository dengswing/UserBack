using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace com.shinezone.network
{
    /// <summary>
    /// udp 发送
    /// </summary>
    public class UdpNetwork : Network<UdpNetwork>
    {
        private Queue<ByteBuffer> buffer;
        private Socket socketClient;
        private IPEndPoint iPEndPoint;
        private Action<int, ByteBuffer> resultBack;
        private EndPoint serverEnd; //服务端

        protected override void Init()
        {
            base.Init();
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            buffer = new Queue<ByteBuffer>();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(ByteBuffer data)
        {
            if (data == null) return;
            buffer.Enqueue(data);
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="resultBack"></param>
        public void ConnectServer(string host, int port, Action<int, ByteBuffer> resultBack)
        {
            this.resultBack = resultBack;
            iPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            serverEnd = (EndPoint)sender;
            if (!isStart) Initialise();
        }

        protected override bool Send()
        {
            base.Send();

            if (buffer.Count <= 0) return true;
            var byteBuff = buffer.Dequeue();

            if (byteBuff == null) return true;
            var tByte = byteBuff.ToBytes();

            try
            {
                socketClient.SendTo(tByte, tByte.Length, SocketFlags.None, iPEndPoint);
                UnityEngine.Debug.Log("send:" + iPEndPoint);
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("send error!");
            }

            return true;
        }
        
        protected override bool Recv()
        {
            base.Recv();          

            var tBuffer = new ByteBuffer();
            byte[] tByte = new byte[1024];
            try
            {
                socketClient.ReceiveFrom(tByte, ref serverEnd);
                UnityEngine.Debug.Log("recv:" + tByte);
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("recv error!");
            }

            if (tByte != null)
                tBuffer = new ByteBuffer(tByte);

            if (resultBack != null) resultBack(1, tBuffer);

            return true;
        }
    }
}
