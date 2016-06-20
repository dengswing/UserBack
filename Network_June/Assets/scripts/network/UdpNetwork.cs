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
        private UdpClient socketClient;
        private IPEndPoint iPEndPoint;
        private Action<int, ByteBuffer> resultBack;


        protected override void Init()
        {
            base.Init();
            socketClient = new UdpClient();
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

            if (!isStart) Initialise();
        }

        protected override void Send()
        {
            base.Send();

            var byteBuff = buffer.Dequeue();
            if (byteBuff == null) return;
            var tByte = byteBuff.ToBytes();
            socketClient.Send(tByte, tByte.Length, iPEndPoint);
        }

        protected override void Recv()
        {
            base.Recv();

            IPEndPoint anyIP = null;
            var tByte = socketClient.Receive(ref anyIP);
            if (resultBack != null) resultBack(1, new ByteBuffer(tByte));
        }
    }
}
