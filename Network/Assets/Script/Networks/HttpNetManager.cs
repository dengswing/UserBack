//#define DEBUG_CONSOLE  //开关debug控制台

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Networks.interfaces;
using Networks.parser;
using System.ComponentModel;

namespace Networks
{
    #region debug
#if UNITY_EDITOR
    public enum StatusType
    {
        /// <summary>
        /// 正常流程
        /// </summary>
        NORMAL,
        /// <summary>
        /// 请求超时
        /// </summary>
        NET_TIME_OUT,
        /// <summary>
        /// 服务器连接失败
        /// </summary>
        CONNECT_ERROR,
        /// <summary>
        /// 后端返回失败
        /// </summary>
        SERVER_ERROR
    }
#endif
    #endregion

    /// <summary>
    /// http请求,IHttpNetManager接口里面是可以外部访问的
    /// </summary>
    public class HttpNetManager : Utilities.SingleInstance<HttpNetManager>,IHttpNetManager
    {
        #region public property
        /// <summary>
        /// 返回的code结构是成功的
        /// </summary>
        public const int RESPONSE_CODE_RESULT_SUCCESS = 0;
        #endregion

        enum RequestState
        {
            [Description("初始状态")]
            State_None = 0,
            [Description("请求中状态")]
            State_Requesting = 1,
            [Description("完成")]
            State_Complete = 2
        }

        #region private property
        /// <summary>
        /// 请求回调委托
        /// </summary>
        Dictionary<string, HttpNetResultDelegate> onResponseDict = new Dictionary<string, HttpNetResultDelegate>();

        //用户id
        string _userID;

        //本地时间
        DateTime localTime = DateTime.Now;

        // 游戏服务器时间（秒为单位）
        long _serverTime;

        // 队列组装数据
        QueueDataGroupManager queueDataGroup;

        //定时器
        NetTimerManager netTimer;

        //数据解析
        INetworkDataParse netParser;

        //是否连接error
        bool isContentError;

        //调试控制台
#if DEBUG_CONSOLE
        IDebugConsole _debugConsole;
#endif
        //是否第一次请求
        bool isFirstPost = true;
        ///==============================================================================================
        /// <summary>
        /// 请求地址
        /// </summary>
        string _requestURL = "http://dev-soul.shinezoneapp.com/?dev=xiuyun&{0}";

        /// <summary>
        /// 发送request最大组合接口包
        /// </summary>
        uint _requestGroupMax = 5;

        /// <summary>
        /// 等待请求时间（单位秒）
        /// </summary>
        float _waitResponseTime = 5;

        /// <summary>
        /// 最大重发次数
        /// </summary>
        uint _resetSendMax = 3;

        //是否激活debug
        bool _isActiveDebug = true;

        //请求的记录
        Dictionary<string, RequestState> requestState = new Dictionary<string, RequestState>();

        /// <summary>
        /// 服务器异常
        /// </summary>
        HttpNetResultDelegate _serverErrorResponse;

        /// <summary>
        /// 网络超时
        /// </summary>
        NetTimerDelegate _netTimeOut;

        /// <summary>
        /// 网络请求状态侦听
        /// </summary>
        NetSendStateDelegate _netSendState;
        ///==============================================================================================
        #endregion

#if UNITY_EDITOR
        [SerializeField]
        StatusType _statusType;        
        public StatusType statusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }
#endif
        #region get set property
        /// <summary>
        /// 用户id
        /// </summary>
        public string userID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                if (null != queueDataGroup) queueDataGroup.userID = _userID;
            }
        }

        /// <summary>
        /// 用户token
        /// </summary>
        public string token
        {
            get { return (null != queueDataGroup ? queueDataGroup.token : string.Empty); }
            set
            {
                if (null != queueDataGroup) queueDataGroup.token = value;
            }
        }

        /// <summary>
        /// 通信地址
        /// </summary>
        public string requestURL
        {
            get { return _requestURL; }
            set
            {
                _requestURL = value;
                if (null != queueDataGroup) queueDataGroup.requestURL = _requestURL;
            }
        }

        /// <summary>
        /// 发送request最大组合接口包
        /// </summary>
        public uint requestGroupMax
        {
            get { return _requestGroupMax; }
            set
            {
                _requestGroupMax = value;
                if (null != queueDataGroup) queueDataGroup.requestGroupMax = _requestGroupMax;
            }
        }

        /// <summary>
        /// 等待请求时间（单位秒）
        /// </summary>
        public float waitResponseTime
        {
            get { return _waitResponseTime; }
            set
            {
                _waitResponseTime = value;
                if (null != netTimer) netTimer.waitResponseTime = _waitResponseTime;
            }
        }

        /// <summary>
        /// 最大重发次数
        /// </summary>
        public uint resetSendMax
        {
            get { return _resetSendMax; }
            set
            {
                _resetSendMax = value;
                if (null != netTimer) netTimer.resetSendMax = resetSendMax;
            }
        }

        /// <summary>
        /// hamc和php约定的key值
        /// </summary>
        public string hamcKey
        {
            get { return (null != queueDataGroup ? queueDataGroup.hmacKey : string.Empty); }
            set
            {
                if (null != queueDataGroup) queueDataGroup.hmacKey = value;
            }
        }

        /// <summary>
        /// 是否激活debug
        /// </summary>
        public bool isActiveDebug
        {
            get { return _isActiveDebug; }
            set { _isActiveDebug = value; }
        }

        /// <summary>
        /// 服务器时间
        /// </summary>
        public long gmt
        {
            get { return serverTime; }
        }
        #endregion

        #region register delegate
        /// <summary>
        /// 注入数据解析类,不注入默认使用框架里面的
        /// </summary>
        /// <param name="dataParse">Data parse.</param>
        public void RegisterNetworkDataParse(INetworkDataParse dataParse)
        {
            netParser = dataParse;
        }

        /// <summary>
        /// 注册委托,数据请求不管成功还是失败都会触发消息通知
        /// </summary>
        /// <param name="commandID">根据后端的接口名称注册侦听</param>
        /// <param name="callback">委托回调方法</param>
        public void RegisterResponse(string commandID, HttpNetResultDelegate callback)
        {
            if (onResponseDict.ContainsKey(commandID))
                onResponseDict[commandID] += callback;
            else
                onResponseDict[commandID] = callback;
        }

        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="commandID">根据后端的接口名称注册侦听</param>
        /// <param name="callback">委托回调方法</param>
        public void RemoveResponse(string commandID, HttpNetResultDelegate callback)
        {
            if (onResponseDict.ContainsKey(commandID))
                onResponseDict[commandID] -= callback;
        }

        /// <summary>
        /// 注入数据表结构
        /// </summary>
        /// <param name="dataTable"></param>
        public void RegisterTableDataStruct(AbsTableDataStruct dataTable)
        {
            TableDataManager.Instance.tableDataStruct = dataTable;
        }
        
        /// <summary>
        /// 服务器异常
        /// </summary>
        public HttpNetResultDelegate serverErrorResponse { get { return _serverErrorResponse; } set { _serverErrorResponse = value; } }

        /// <summary>
        /// 网络超时
        /// </summary>
        public NetTimerDelegate netTimeOut { get { return _netTimeOut; } set { _netTimeOut = value; } }

        /// <summary>
        /// 网络请求状态侦听
        /// </summary>
        public NetSendStateDelegate netSendState { get { return _netSendState; } set { _netSendState = value; } }

        #endregion

        #region post request
        /// <summary>
        /// 打包请求
        /// </summary>
        /// <param name="commandId">后端的接口名称</param>
        /// <param name="c">需要传递的参数</param>
        public void Post(string commandId, params object[] c)
        {
            List<object> args = new List<object>(c);
            queueDataGroup.AddRequest(commandId, args);
        }

        /// <summary>
        /// 打包请求
        /// </summary>
        /// <param name="commandId">后端的接口名称</param>
        /// <param name="resultBack">结果委托,报了系统级别错误不会有回调</param>
        /// <param name="c">需要传递的参数</param>
        public void Post(string commandId, HttpNetResultDelegate resultBack, params object[] c)
        {
            List<object> args = new List<object>(c);
            queueDataGroup.AddRequest(commandId, args, resultBack);
        }

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="c">参数</param>
        public void PostOneToOne(string commandId, params object[] c)
        {
            List<object> args = new List<object>(c);
            SendOneToOne(commandId, args, null, null);
        }

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="resultBack">回调委托</param>
        /// <param name="c">参数</param>
        public void PostOneToOne(string commandId, HttpNetResultDelegate resultBack, params object[] c)
        {
            List<object> args = new List<object>(c);
            SendOneToOne(commandId, args, resultBack, null);
        }

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="url">请求路径</param>
        /// <param name="c">参数</param>
        public void PostOneToOne(string commandId, string url, params object[] c)
        {
            List<object> args = new List<object>(c);
            SendOneToOne(commandId, args, null, url);
        }

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="url">请求路径</param>
        /// <param name="resultBack">回调委托</param>
        /// <param name="c">参数</param>
        public void PostOneToOne(string commandId, string url, HttpNetResultDelegate resultBack, params object[] c)
        {
            List<object> args = new List<object>(c);
            SendOneToOne(commandId, args, resultBack, url);
        }

        /// <summary>
        /// 启动重新在发送请求
        /// </summary>
        public void StartResetSend()
        {
            //超时了允许重发
            if (netTimer.isTimeOut)
            {
                isContentError = false;
                ResetSendRequest(false);
            }
            else
            {
                DebugTrace("HttpNetManager::ResetSend :Error:No timeout, don't need to resend!");
            }
        }
        #endregion

        #region clear 
        /// <summary>
        /// 清除所有未请求的数据
        /// </summary>
        public void Clear()
        {
            RemoveAllRequset();
        }

        /// <summary>
        /// 清除所有未请求的数据
        /// </summary>
        /// <param name="sendAllData">是否发送所有未发送的请求</param>
        public void Clear(bool isSendAllData)
        {
            if (isSendAllData)
            {
                string URL = queueDataGroup.AllRequestData(serverTime);
                if (URL != null) StartCoroutine(PostSingle(null, URL, null));
            }

            Clear();
        }
        
        /// <summary>
        /// 重置本地时间
        /// </summary>
        public void ResetLocalTime()
        {
            localTime = DateTime.Now;
        }
        #endregion

        private HttpNetManager()
        {
            Init();
        }

        void Awake()
        {

#if DEBUG_CONSOLE
            _debugConsole = Networks.log.DebugConsole.Instance;
#endif
        }

        void Update()
        {
            if (netTimer != null) netTimer.UpdateTime(); //更新定时器时间
        }

        void LateUpdate()
        {
            SendRequest();
        }

        #region init 
        void Init()
        {
            queueDataGroup = new QueueDataGroupManager();
            netTimer = new NetTimerManager();
            netTimer.resetSend = ResetSend;
            netTimer.netTimeOut = NetTimeOut;

            queueDataGroup.requestURL = requestURL;
            queueDataGroup.requestGroupMax = requestGroupMax;
            netTimer.waitResponseTime = waitResponseTime;
            netTimer.resetSendMax = resetSendMax;
        }
        #endregion

        /// <summary>
        /// 游戏服务器当前时间（秒为单位）
        /// </summary>
        long serverTime
        {
            get
            {
                TimeSpan tSpan = DateTime.Now.Subtract(localTime);
                return _serverTime + (long)tSpan.TotalSeconds;
            }
            set
            {
                _serverTime = value;
                localTime = DateTime.Now;
            }
        }

        void RemoveAllRequset()
        {
            StopCoroutine("PostAsync");
            queueDataGroup.Clear();
            netTimer.Clear();
            DebugTrace("HttpNetManager::ResetSend :Error:Remove all request!");
            isContentError = false;
            requestState.Clear();
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        void SendRequest()
        {
            if (isContentError)
            {   //上一次请求断网、继续尝试下一次请求
                if (netTimer.isTimeOut || netTimer.isRunning) return;                
                netTimer.StartTime(); //计时开始
                ResetSendRequest(); //重发 
                return;
            }

            if (!queueDataGroup.isCanSend) return; //没有请求

            if (isFirstPost)
            { //第一次时间为0
                isFirstPost = false;
                ResetLocalTime();
            }

            IPostData postData = queueDataGroup.GroupPostDataPack(serverTime);
            if (postData == null) return;
            StartCoroutine(PostAsync(postData));
        }

        void ResetSendRequest(bool isReset = true)
        {
            IPostData postData = queueDataGroup.currentPostData;
            if (postData == null)
            {
                DebugTrace("HttpNetManager::ResetSend :Error:No data can be sent!");
                return;
            }

            DebugTrace("HttpNetManager::ResetSend :reset send commandId:[" + postData.ToString() + "]" + isReset);
            StartCoroutine(PostAsync(postData, isReset));
        }

        /// <summary>
        /// 发送一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="url">请求路径</param>
        /// <param name="resultBack">回调委托</param>
        /// <param name="c">参数</param>
        void SendOneToOne(string commandId, List<object> args, HttpNetResultDelegate resultBack, string url)
        {
            if (url == null) url = requestURL;
            string URL = queueDataGroup.OneToOnePostDataPack(commandId, args, serverTime, url);
            StartCoroutine(PostSingle(commandId, URL, resultBack));
        }

        /// <summary>
        /// 重发起请求
        /// </summary>
        void ResetSend()
        {
            ResetSendRequest();
        }

        /// <summary>
        /// 超时
        /// </summary>
        void NetTimeOut()
        {
            StopCoroutine("PostAsync");
            DebugTrace("HttpNetManager::NetTimeOut Error :network time out!");
            if (null != netTimeOut) netTimeOut();
        }

        #region post request
        /// <summary>
        /// 请求http
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerator PostAsync(IPostData data, bool isReset = false)
        {
            if (netTimer.isTimeOut) yield break;    //超时退出请求
            string url = data.url;
            var requestKey = RequestOnlyKey(url);
            if (CheckRequestState(requestKey)) yield break;   //已结束

            if (!isReset) netTimer.StartTime(); //计时开始
           

#if UNITY_EDITOR
            if (statusType == StatusType.CONNECT_ERROR) url = "content::" + url;
#endif


#if DEBUG_CONSOLE
            var localTime = DateTime.Now;
#endif

            if (null != netSendState) netSendState(true);
            DebugTrace(">>:" + url);
            WWW www = new WWW(url);
            yield return www;
            if (null != netSendState) netSendState(false);

            if (CheckRequestState(requestKey)) yield break;   //已结束
            requestState[requestKey] = RequestState.State_Complete;

#if UNITY_EDITOR
            if (statusType == StatusType.NET_TIME_OUT) yield break;
#endif

#if DEBUG_CONSOLE
            TimeSpan tSpan = DateTime.Now.Subtract(localTime);
            DebugTrace("post back time=" + tSpan.Milliseconds + " << " + url.Substring(url.IndexOf("*=") + 1), true, false);
            localTime = DateTime.Now;
#endif

            DebugTrace("<< [" + data.ToString() + "]:" + www.text, false);
            
            if (netTimer.isTimeOut) yield break;    //超时退出请求,防止超时了还回来数据

            if (www.error != null)
            { //处理404  todo  断网应该如何处理
                DebugTrace("HttpNetManager::PostAsync Error:" + www.error);
                if(null != serverErrorResponse) serverErrorResponse(www.error, -1, "error");
                if(!isContentError) netTimer.StopTime(); //计时停止
                isContentError = true;
                yield break;
            }
            else
            {
                isContentError = false;
            }

            netTimer.StopTime(); //计时停止
            queueDataGroup.FinishPost();    //完成了本次请求

            string result = www.text;
            int res = -1;
            string errMsg = "";
            string msg = "";

#if UNITY_EDITOR
            if (statusType == StatusType.SERVER_ERROR) result = "{\"code\":10511,\"msg\":\"CommonVoList: Element 1 cannot be found in MissionLastVoList.\",\"gmt\":1452475369}";
#endif

            if (netParser == null)
            {
                Debug.LogError("HttpNetManager::PostAsync Error : netParser Can't be empty!");
            }

            res = netParser.ParseData(result, out errMsg, out msg);
            serverTime = netParser.serverTime;

#if DEBUG_CONSOLE
            tSpan = DateTime.Now.Subtract(localTime);
            DebugTrace("post parse time=" + tSpan.Milliseconds + " << " + url.Substring(url.IndexOf("*=") + 1), true, false);
            DebugTrace(result, true, false, false);

#endif
            errMsg = url;
            TriggerResponse(data, res, msg, errMsg);

            if (res == RESPONSE_CODE_RESULT_SUCCESS && queueDataGroup.NextSendRequest)
            { //进入下一组
                SendRequest();
            }
        }

        #region request state
        string RequestOnlyKey(string url)
        {
            var split = url.Split(new char[1] { '&' });
            foreach (var key in split)
            {
                if (key.IndexOf("sign") != -1) return  key;
            }
            return null;
        }
        bool CheckRequestState(string key)
        {
            if (requestState.ContainsKey(key))
            {
                return (requestState[key] == RequestState.State_Complete);
            }
            else
            {
                requestState.Add(key, RequestState.State_Requesting);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 触发回调委托
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res"></param>
        /// <param name="result"></param>
        /// <param name="errMsg"></param>
        /// <param name="isError"></param>
        void TriggerResponse(IPostData data, int res, string result, string errMsg, bool isError = false)
        {
            if (serverErrorResponse != null && res != RESPONSE_CODE_RESULT_SUCCESS)
            { //异常先抛出、会继续回调
                DebugTrace("HttpNetManager::TriggerResponse Error : commandId:[" + data.ToString() + "]error:" + errMsg);
                serverErrorResponse(errMsg, res, result);
            }

            int count = data.commandId.Count;
            string commandId;
            HttpNetResultDelegate resultBack;
            for (int i = 0; i < count; i += 1)
            {
                commandId = data.commandId[i];
                if (onResponseDict.ContainsKey(commandId) && onResponseDict[commandId] != null)
                {
                    onResponseDict[commandId](commandId, res, result);
                }

                if (!isError)
                {
                    resultBack = data.resultBack[i];
                    if (resultBack != null)
                    {
                        resultBack(commandId, res, result);
                    }
                }
            }
        }

        /// <summary>
        /// 请求http,单一发送，不需求得到结果处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerator PostSingle(string commandId, string url, HttpNetResultDelegate resultBack)
        {
            DebugTrace("singlePost>>:" + url);
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                DebugTrace("singlePost:<< Error:" + www.error);
                yield break;
            }

            if (resultBack != null)
            {
                resultBack(commandId, RESPONSE_CODE_RESULT_SUCCESS, www.text);
            }

            DebugTrace("singlePost<< " + url + ":" + www.text, false);
            DebugTrace(www.text, true, false);
        }
        #endregion

        #region debug
        void DebugTrace(string msg, bool isConsole = true, bool isLog = true, bool isTime = true)
        {
            if (!isActiveDebug) return;
            if (isLog) Debug.Log(msg);

#if DEBUG_CONSOLE
            if (_debugConsole != null && isConsole) _debugConsole.Log(msg, isTime);
#endif

        }
        #endregion
    }
}