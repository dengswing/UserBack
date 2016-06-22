using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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

            HttpGet(url);
        }

        protected override bool Send()
        {
            return base.Send();
        }

        protected override bool Recv()
        {
           return base.Recv(); 
        }

        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
          //  request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

          //  response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            Debug.Log("retString==>" + retString);
            return retString;
        }

    }
}
