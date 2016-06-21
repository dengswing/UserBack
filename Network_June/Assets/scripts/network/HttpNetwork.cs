using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.shinezone.network
{
    /// <summary>
    /// http 发送
    /// </summary>
    public class HttpNetwork : Network<HttpNetwork>
    {
        private Queue<string> buffer;
        private Action<int, object> resultBack;
        private WWW httpPost;
        private bool isRecvData = true;


        protected override void Init()
        {
            base.Init();
            buffer = new Queue<string>();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(string url, Action<int, object> resultBack)
        {
            if (url == null) return;
            buffer.Enqueue(url);
            this.resultBack = resultBack;

            if (!isStart) Initialise();

            SendHttp();
        }

        private void SendHttp()
        {
            if (!isRecvData || buffer.Count <= 0) return;
            var url = buffer.Dequeue();
            if (url == null) return;

            httpPost = new WWW(url);
            isRecvData = false;
        }

        protected override void Send()
        {
            base.Send();
        }

        int updateStep = 1;
        float tm = 0f;
        protected override void Recv()
        {
            base.Recv();
            if (httpPost == null) return;

            if (0 < updateStep)
            {
                tm += Time.deltaTime;
                if (tm >= 2)
                {
                    tm = 0f;
                    // Call custom update
                    DataDispose();
                }
            }
            else {
                // Call custom update
                DataDispose();
            }
        }

        private void DataDispose()
        {
            if (httpPost.error != null)
            {
                httpPost.Dispose();
                isRecvData = true;
                if (resultBack != null) resultBack(0, null);
            }
            else if (httpPost.isDone)
            {
                var data = httpPost.text;
                httpPost.Dispose();
                isRecvData = true;
                if (resultBack != null) resultBack(1, data);
            }

            httpPost = null;
            SendHttp();
        }
    }
}
