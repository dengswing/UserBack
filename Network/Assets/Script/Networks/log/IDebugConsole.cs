namespace Networks.interfaces
{
    public interface IDebugConsole
    {
        //手机调试log
        void Log(string msg);
        void Log(string msg, bool isTime);
    }
}
