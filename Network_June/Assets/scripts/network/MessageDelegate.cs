namespace com.shinezone.network
{
    /// <summary>
    /// http请求结果委托
    /// </summary>
    /// <param name="cmd">接口名字</param>
    /// <param name="res">0:成功,其他值:失败</param>
    /// <param name="value">请求返回的结果</param>
    public delegate void SocketResultDelegate(string cmd, int res, string value);
}