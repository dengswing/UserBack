﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Networks.interfaces;
using Networks.parser;
using Networks.data;

namespace Networks
{
    /// <summary>
    /// 队列组装数据
    /// </summary>
    internal class QueueDataGroupManager
    {
        /// <summary>
        /// 发送request最大包组合
        /// </summary>
        public uint requestGroupMax = 5;

        //用户id
        public string userID;

        /// <summary>
        /// 请求地址
        /// </summary>
        public string requestURL;

        /// <summary>
        /// 接口参数
        /// </summary>
        string requestParams = "[\"{0}\",[{1}]]";


        PostData _currentPostData;
        Queue groupData;

        public QueueDataGroupManager()
        {
            groupData = new Queue();
        }

        /// <summary>
        /// 当前请求数据
        /// </summary>
        public IPostData currentPostData
        {
            get
            {
                return _currentPostData;
            }
        }

        /// <summary>
        /// 添加请求
        /// </summary>
        /// <param name="commandId">后端的接口名称</param>
        /// <param name="param">需要传递的参数</param>
        /// <param name="resultBack">结果委托</param>
        public void AddRequest(string commandId, List<object> param, HttpNetResultDelegate resultBack = null)
        {
            ArrayList data = new ArrayList(3);
            data.Add(commandId);
            data.Add(param);
            data.Add(resultBack);
            groupData.Enqueue(data);
        }

        /// <summary>
        /// 是否可以发生请求
        /// </summary>
        public bool isCanSend
        {
            get
            {
                return (null == currentPostData);
            }
        }

        /// <summary>
        /// 组装请求数据包装
        /// </summary>
        /// <param name="time">服务器时间</param>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public IPostData GroupPostDataPack(long time)
        {
            if (groupData.Count <= 0) return null;

            _currentPostData = new PostData();
            _currentPostData.commandId = new List<string>();
            _currentPostData.resultBack = new List<HttpNetResultDelegate>();
            string urlParams = PostParamGroup(_currentPostData, time, userID, requestParams);
            _currentPostData.url = string.Format(requestURL, urlParams); //url组装
            return _currentPostData as IPostData;
        }

        /// <summary>
        /// 一对一请求数据包装
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="args">参数</param>
        /// <param name="time">服务器时间</param>
        /// <returns></returns>
        public string OneToOnePostDataPack(string commandId, List<object> args, long time)
        {
            string urlParams = ParamtersPack(commandId, args, time, userID, requestParams);
            return string.Format(requestURL, urlParams); //url组装
        }

        /// <summary>
        /// 一对一请求数据包装
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="args">参数</param>
        /// <param name="time">服务器时间</param>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string OneToOnePostDataPack(string commandId, List<object> args, long time, string url)
        {
            string urlParams = ParamtersPack(commandId, args, time, userID, requestParams);
            return string.Format(url, urlParams); //url组装
        }

        /// <summary>
        /// 所有的请求
        /// </summary>
        /// <param name="time">服务器时间</param>
        /// <returns></returns>
        public string AllRequestData(long time)
        {
            if (groupData.Count <= 0) return null;
            string urlParams = PostParamGroup(null, time, userID, requestParams, true);
            if (urlParams == null) return null;

            return string.Format(requestURL, urlParams); //url组装
        }

        /// <summary>
        /// 进行下一组请求
        /// </summary>
        /// <returns>true:应许</returns>
        public bool NextSendRequest
        {
            get
            {
                return (groupData.Count > 0);
            }
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            groupData.Clear();
            FinishPost();
        }

        /// <summary>
        /// 完成了单次请求
        /// </summary>
        public void FinishPost() 
        {
            _currentPostData = null;
        }

        /// <summary>
        /// 组装请求参数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="time">服务器时间</param>
        /// <param name="userId">用户id</param>
        /// <param name="param">参数组装格式</param>
        /// <returns></returns>
        string PostParamGroup(IPostData data, long time, string userId, string param, bool isAll = false)
        {
            string urlValue = "";
            int count = (isAll ? groupData.Count : (int)Mathf.Min((float)requestGroupMax, (float)groupData.Count));
            for (int i = 0; i < count; i += 1)
            {
                ArrayList tmpData = groupData.Dequeue() as ArrayList; //拉取一个
                List<object> args = tmpData[1] as List<object>;
                string commandId = tmpData[0] as string;
                urlValue += ParamtersPack(commandId, args, time, userId, param);  //参数组装

                if (null != data && !data.commandId.Contains(commandId))
                {
                    data.commandId.Add(commandId);
                    data.resultBack.Add((HttpNetResultDelegate)tmpData[2]);
                }

                if (i != count - 1)
                {
                    urlValue += ",";
                }
            }

            return urlValue;
        }

        /// <summary>
        /// 参数组装
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="args">参数</param>
        /// <param name="time">服务器时间</param>
        /// <param name="userId">用户id</param>
        /// <param name="param">参数组装格式</param>
        /// <returns></returns>
        string ParamtersPack(string commandId, List<object> args, long time, string userId, string param)
        {
            args.Insert(0, time);
            args.Insert(0, 0);
            args.Insert(0, userId);

            string value = MakeParamters(args.ToArray());
            return string.Format(param, commandId, value);
        }

        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        string MakeParamters(object[] args)
        {
            string str = "";
            for (int i = 0; i < args.Length; i++)
            {
                object argv = args[i];
                if (argv.GetType() == typeof(string))
                {
                    str += "\"" + argv + "\"";
                }
                else if (argv is IList && argv.GetType().IsGenericType)
                {
                    var list = argv as IList;
                    var objs = new object[list.Count];
                    list.CopyTo(objs, 0);
                    str += "[" + this.MakeParamters(objs) + "]";
                }
                else
                {
                    str += argv;
                }

                if (i < args.Length - 1)
                {
                    str += ",";
                }
            }
            return str;
        }
    }
}