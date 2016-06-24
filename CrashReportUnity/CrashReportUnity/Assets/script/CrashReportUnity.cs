using System.Runtime.InteropServices;

public class CrashReportUnity
{
    [DllImport("__Internal")]
    private static extern void _Init(string gameId, string cpId, string cpKey, string szId, string userId);

    public void init(string gameId, string cpId, string cpKey, string szId, string userId)
    {
        _Init(gameId, cpId, cpKey, szId, userId);
    }
}
