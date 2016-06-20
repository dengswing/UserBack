using System;
using System.Collections.Generic;

namespace com.shinezone.network
{
    /// <summary>
    /// tcp 发送
    /// </summary>
    public class TcpNetwork : Network<TcpNetwork>
    {
        private Queue<ByteBuffer> buffer;
        private SocketClient socketClient;

        protected override void Init()
        {
            base.Init();
            socketClient = new SocketClient();
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
            ConnectClient(host, port, resultBack);
        }

        private void ConnectClient(string host, int port, Action<int, ByteBuffer> resultBack)
        {
            socketClient.SendConnect(host, port);
            socketClient.OnRegister(resultBack);
        }

        protected override void Send()
        {
            base.Send();

            var byteBuff = buffer.Dequeue();
            if (byteBuff == null) return;
            socketClient.SendMessage(byteBuff);
            byteBuff = null;
        }

        protected override void Recv()
        {
            base.Recv();
        }
    }
}
