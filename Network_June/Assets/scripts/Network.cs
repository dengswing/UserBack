using System.Threading;

namespace com.shinezone.network
{
    /// <summary>
    /// 网络请求父类
    /// </summary>
    public class Network
    {
        public static readonly Network Instance = new Network();

        private Thread sendThread;
        private Thread recvThread;
        static readonly object sendLockObj = new object();
        static readonly object recvLockObj = new object();

        public Network()
        {
            Init();
        }

        /// <summary>
        /// 初始化，启动
        /// </summary>
        public void Initialise()
        {
            Start();
        }

        /// <summary>
        /// 停止工作
        /// </summary>
        public void Shutdown()
        {
            sendThread.Abort();
            recvThread.Abort();
        }

        #region 子类必须重载
        /// <summary>
        /// 发送消息，子类必须重载
        /// </summary>
        public virtual void Send()
        {

        }

        /// <summary>
        /// 接收信息，子类必须重载
        /// </summary>
        public virtual void Recv()
        {

        }
        #endregion

        private void Start()
        {
            sendThread.Start();
            recvThread.Start();
        }

        private void Init()
        {
            sendThread = new Thread(SendHandler);
            recvThread = new Thread(RecvHandler);
        }

        #region 线程处理

        private void SendHandler()
        {
            while (true)
            {
                lock (sendLockObj)
                {
                    try
                    {
                        Send();
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError("SendHandler:" + ex.Message);
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void RecvHandler()
        {
            while (true)
            {
                lock (recvLockObj)
                {
                    try
                    {
                        Recv();
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError("RecvHandler:" + ex.Message);
                    }
                }
                Thread.Sleep(1);
            }
        }

        #endregion
    }
}