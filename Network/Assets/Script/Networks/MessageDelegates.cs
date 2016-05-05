namespace Networks
{
    /// <summary>
    /// 网络时间委托
    /// </summary>
    public delegate void NetTimerDelegate();
    /// <summary>
    /// 简单时间委托
    /// </summary>
    public delegate void SimpleTimerDelegate();

    /// <summary>
    /// 队列委托
    /// </summary>
    public delegate void QueueDataGroupDelegate();

    /// <summary>
    /// http请求结果委托
    /// </summary>
    /// <param name="cmd">接口名字</param>
    /// <param name="res">0:成功,其他值:失败</param>
    /// <param name="value">请求返回的结果</param>
    public delegate void HttpNetResultDelegate(string cmd, int res, string value);

    /// <summary>
    /// 数据变更委托
    /// </summary>
    /// <param name="data">数据</param>
    public delegate void DataTableUpdateDelegate(object data);

    /// <summary>
    /// 删除表内容通知
    /// </summary>
    /// <param name="tableName">表名</param>
    public delegate void DeleteTableDelegate(string tableName);

    /// <summary>
    /// 网络请求的状态委托
    /// </summary>
    /// <param name="isStart">是否开始发送请求，true:开始发送、false:结束发送</param>
    public delegate void NetSendStateDelegate(bool isStart);
}