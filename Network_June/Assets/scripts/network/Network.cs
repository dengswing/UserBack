using System.Threading;

namespace com.shinezone.network
{
    /// <summary>
    /// 网络请求父类
    /// </summary>
    public class Network<T> where T : Network<T>, new()
    {
        #region 实例化
        private static T sInstance;
        private static object sLock = new object();
        public static T Instance
        {
            get
            {
                lock (sLock)
                {
                    if (sInstance == null)
                    {
                        sInstance = new T();
                    }

                    return sInstance;
                }
            }
        }
        #endregion

        private Thread sendThread;
        private Thread recvThread;
        static readonly object sendLockObj = new object();
        static readonly object recvLockObj = new object();
        protected bool isStart;

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

            isStart = false;
        }

        /// <summary>
        /// 发起请求
        /// </summary>
        public void Post()
        {

        }

        #region 子类必须重载
        /// <summary>
        /// 发送消息
        /// </summary>
        protected virtual void Send()
        {

        }

        /// <summary>
        /// 接收信息
        /// </summary>
        protected virtual void Recv()
        {

        }
        #endregion

        private void Start()
        {
            if (isStart) return;
            sendThread.Start();
            recvThread.Start();
            isStart = true;
        }

        protected virtual void Init()
        {
            sendThread = new Thread(SendHandler);
            recvThread = new Thread(RecvHandler);
        }

        #region 线程处理

        private void SendHandler()
        {
            while (true)
            {
                try
                {
                    Send();
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError("SendHandler:" + ex.Message);
                }

                Thread.Sleep(1);
            }
        }

        private void RecvHandler()
        {
            while (true)
            {
                try
                {
                    Recv();
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError("RecvHandler:" + ex.Message);
                }

                Thread.Sleep(1);
            }
        }

        #endregion
    }
}