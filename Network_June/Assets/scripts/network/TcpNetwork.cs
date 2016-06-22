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

        protected override bool Send()
        {
            base.Send();

            if (buffer.Count <= 0) return true;
            var byteBuff = buffer.Dequeue();
            if (byteBuff == null) return true;
            SendMessageBegin(byteBuff);
            byteBuff = null;
            return true;
        }

        protected override bool Recv()
        {
            base.Recv();

            if (!CheckConnected())
            {
                return false;
            }

            //接受数据保存至bytes当中  
            byte[] bytes = new byte[1024 * 1024 * 2];
            try
            {
                int receiveNumber = socketClient.Receive(bytes);
                string strMsg = System.Text.Encoding.UTF8.GetString(bytes);// 将接受到的字节数据转化成字符串；  
                Debug.Log("收到服务器的消息=====>>>" + strMsg);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to clientSocket error." + e);
                socketClient.Close();
                return false;
            }

            if (resultBack != null) resultBack(1,new ByteBuffer(bytes));
            return true;
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

        private bool CheckConnected()
        {
            if (!socketClient.Connected)
            {
                Debug.Log("Failed to clientSocket server.");
                socketClient.Close();
                return false;
            }

            return true;
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

            if (!CheckConnected())
            {
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

        //关闭Socket  
        private void Closed()
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
