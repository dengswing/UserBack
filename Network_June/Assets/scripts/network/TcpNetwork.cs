using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Debug = UnityEngine.Debug;

namespace com.shinezone.network
{
    /// <summary>
    /// tcp 发送
    /// </summary>
    public class TcpNetwork : Network<TcpNetwork>
    {
        private Queue<ByteBuffer> buffer;
        private Socket socketClient;
        private Action<int, ByteBuffer> resultBack;

        protected override void Init()
        {
            base.Init();
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
            this.resultBack = resultBack;

            //服务器IP地址  
            IPAddress address = IPAddress.Parse(host);
            //服务器端口  
            IPEndPoint endpoint = new IPEndPoint(address, port);

            //异步连接,连接成功调用connectCallback方法  
            IAsyncResult result = socketClient.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), socketClient);

            //这里做一个超时的监测，当连接超过5秒还没成功表示超时  
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                //超时  
                Closed();
                Debug.Log("connect Time Out");
            }

            if (!isStart) Initialise();
        }

        private void ConnectCallback(IAsyncResult asyncConnect)
        {
            Debug.Log("connect success");
        }

        //向服务端发送一条字符串  
        //一般不会发送字符串 应该是发送数据包  
        private void SendMessageBegin(ByteBuffer data)
        {
            byte[] msg = data.ToBytes();

            if (!socketClient.Connected)
            {
                socketClient.Close();
                return;
            }

            try
            {
                IAsyncResult asyncSend = socketClient.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(SendCallback), socketClient);
                bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
                if (!success)
                {
                    socketClient.Close();
                    Debug.Log("Failed to SendMessage server.");
                }
            }
            catch
            {
                Debug.Log("send message error");
            }
        }

        private void SendCallback(IAsyncResult asyncConnect)
        {
            Debug.Log("send success");
        }

        protected override void Send()
        {
            base.Send();

            if (buffer.Count <= 0) return;
            var byteBuff = buffer.Dequeue();
            if (byteBuff == null) return;
            SendMessageBegin(byteBuff);
            byteBuff = null;
        }

        protected override void Recv()
        {
            base.Recv();

            if (!socketClient.Connected)
            {
                //与服务器断开连接跳出循环  
                Debug.Log("Failed to clientSocket server.");
                socketClient.Close();
                return;
            }

            try
            {
                //接受数据保存至bytes当中  
                byte[] bytes = new byte[4096];
                //Receive方法中会一直等待服务端回发消息  
                //如果没有回发会一直在这里等着。  
                int i = socketClient.Receive(bytes);
                if (i <= 0)
                {
                    socketClient.Close();
                }
                Debug.Log(System.Text.Encoding.Default.GetString(bytes));
            }
            catch (Exception e)
            {
                Debug.Log("Failed to clientSocket error." + e);
                socketClient.Close();                
            }
        }

        //关闭Socket  
        public void Closed()
        {
            if (socketClient != null && socketClient.Connected)
            {
                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();
            }
            socketClient = null;
        }
    }
}
