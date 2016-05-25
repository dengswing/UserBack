using UnityEngine;
using System.Collections;

/// <summary>
/// 功能: 日志的测试类
/// 创建: 于立俊
/// 注意：不要在unity中直接双击logServer.exe,否则接受不到日志，不知道什么原因。
/// </summary>


public class TestLog : MonoBehaviour
{
    private log4net.ILog log = KLog.GetLog<TestLog>();

    void OnClick()
    {
        //错误等级逐渐上升
        log.Debug("测试信息");
        log.Info("提示信息");
        log.Warn("警告信息");
        log.Error("一般错误信息");
        log.Fatal("致命错误信息");
    }
}