using System;
using System.Collections.Generic;

namespace jun
{
    class HttpSendData
    {
        public string url;
        public string postData;
        public Action<int, object> resultBack;
        public object result;
    }

    /// <summary>
    /// http 发送
    /// </summary>
    public class HttpNetwork : Network<HttpNetwork>
    {
        private HttpProc.WebClient client;
        private Queue<HttpSendData> queueInfo;
        private bool isSend;
        private HttpSendData currentInfo;

        protected override void Init()
        {
            base.Init();
            client = new HttpProc.WebClient();
            client.Encoding = System.Text.Encoding.Default;//默认编码方式，根据需要设置其他类型
            queueInfo = new Queue<HttpSendData>();
        }

        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="data"></param>
        public void RequestGet(string url, Action<int, object> resultBack)
        {
            RequestPost(url, null, resultBack);
        }

        /// <summary>
        /// post 请求
        /// </summary>
        /// <param name="data"></param>
        public void RequestPost(string url, string postData, Action<int, object> resultBack)
        {
            var sendData = new HttpSendData();
            sendData.url = url;
            sendData.resultBack = resultBack;
            sendData.postData = postData;
            queueInfo.Enqueue(sendData);

            if (!isStart) Initialise();
        }

        protected override bool Send()
        {
            base.Send();

            if (isSend || queueInfo.Count <= 0) return true;
            currentInfo = queueInfo.Dequeue();
            if (currentInfo == null) return true;
            if (currentInfo.postData == null)
            {
                currentInfo.result = client.OpenRead(currentInfo.url);//普通get请求
            }
            else
            {
                currentInfo.result = client.OpenRead(currentInfo.url, currentInfo.postData);//post请求
            }
            isSend = true;
            return true;
        }

        protected override bool Recv()
        {
            base.Recv();
            if (currentInfo == null) return true;
           
            if (currentInfo.result != null)
            {
                if (currentInfo.resultBack != null) currentInfo.resultBack(1, currentInfo.result);
                isSend = false;
                currentInfo = null;
            }
            return true;
        }
    }
}
