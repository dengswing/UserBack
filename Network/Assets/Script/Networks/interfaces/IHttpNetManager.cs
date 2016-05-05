using Networks.parser;

namespace Networks.interfaces
{
    interface IHttpNetManager
    {
        #region base property
        /// <summary>
        /// 用户id
        /// </summary>
        string userID { get; set; }

        /// <summary>
        /// 用户token
        /// </summary>
        string token { get; set; }

        /// <summary>
        /// 通信地址
        /// </summary>
        string requestURL { get; set; }

        /// <summary>
        /// 发送request最大组合接口包
        /// </summary>
        uint requestGroupMax { get; set; }

        /// <summary>
        /// 等待请求时间（单位秒）
        /// </summary>
        float waitResponseTime { get; set; }

        /// <summary>
        /// 最大重发次数
        /// </summary>
        uint resetSendMax { get; set; }

        /// <summary>
        /// hamc和php约定的key值
        /// </summary>
        string hamcKey { get; set; }

        /// <summary>
        /// 是否激活debug
        /// </summary>
        bool isActiveDebug { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        long gmt { get; }

        #endregion

        #region register delegate

        /// <summary>
        /// 服务器异常侦听
        /// </summary>
        HttpNetResultDelegate serverErrorResponse { get; set; }

        /// <summary>
        /// 网络超时侦听
        /// </summary>
        NetTimerDelegate netTimeOut { get; set; }

        /// <summary>
        /// 网络请求状态侦听
        /// </summary>
        NetSendStateDelegate netSendState { get; set; }

        /// <summary>
        /// 注入数据解析类,不注入默认使用框架里面的
        /// </summary>
        /// <param name="dataParse">Data parse.</param>
        void RegisterNetworkDataParse(INetworkDataParse dataParse);

        /// <summary>
        /// 注册委托,数据请求不管成功还是失败都会触发消息通知
        /// </summary>
        /// <param name="commandID">根据后端的接口名称注册侦听</param>
        /// <param name="callback">委托回调方法</param>
        void RegisterResponse(string commandID, HttpNetResultDelegate callback);

        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="commandID">根据后端的接口名称注册侦听</param>
        /// <param name="callback">委托回调方法</param>
        void RemoveResponse(string commandID, HttpNetResultDelegate callback);

        /// <summary>
        /// 注入数据表结构
        /// </summary>
        /// <param name="dataTable"></param>
        void RegisterTableDataStruct(AbsTableDataStruct dataTable);

        #endregion

        #region post request
        /// <summary>
        /// 打包请求
        /// </summary>
        /// <param name="commandId">后端的接口名称</param>
        /// <param name="c">需要传递的参数</param>
        void Post(string commandId, params object[] c);

        /// <summary>
        /// 打包请求
        /// </summary>
        /// <param name="commandId">后端的接口名称</param>
        /// <param name="resultBack">结果委托,报了系统级别错误不会有回调</param>
        /// <param name="c">需要传递的参数</param>
        void Post(string commandId, HttpNetResultDelegate resultBack, params object[] c);

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="c">参数</param>
        void PostOneToOne(string commandId, params object[] c);

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="resultBack">回调委托</param>
        /// <param name="c">参数</param>
        void PostOneToOne(string commandId, HttpNetResultDelegate resultBack, params object[] c);

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="url">请求路径</param>
        /// <param name="c">参数</param>
        void PostOneToOne(string commandId, string url, params object[] c);

        /// <summary>
        /// 一对一请求
        /// </summary>
        /// <param name="commandId">接口名称</param>
        /// <param name="url">请求路径</param>
        /// <param name="resultBack">回调委托</param>
        /// <param name="c">参数</param>
        void PostOneToOne(string commandId, string url, HttpNetResultDelegate resultBack, params object[] c);

        /// <summary>
        /// 启动重新在发送请求
        /// </summary>
        void StartResetSend();

        #endregion

        #region clear 
        /// <summary>
        /// 清除所有未请求的数据
        /// </summary>
        void Clear();

        /// <summary>
        /// 清除所有未请求的数据
        /// </summary>
        /// <param name="sendAllData">是否发送所有未发送的请求</param>
        void Clear(bool isSendAllData);

        /// <summary>
        /// 重置本地时间
        /// </summary>
        void ResetLocalTime();

        #endregion
    }
}
