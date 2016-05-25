
using UnityEngine;

namespace SimpleFramework
{
    public class Log4U : MonoBehaviour
    {
        private log4net.ILog log;
        private static Log4U mInstance = null;

        void Awake()
        {

            log = Log.GetLog<Log4U>();
            mInstance = this;

#if !UNITY_EDITOR
            //在这里做一个Log的监听
            Application.logMessageReceived += HandleLog;
#endif
            // First Log. TODO Add some info...
            log.Debug("DoraTD TODO Info...");


            UnityEngine.Debug.Log("awake");
            log.Debug("测试信息");
            log.Info("提示信息");
            log.Warn("警告信息");
            //log.Error("一般错误信息");
            //log.Fatal("致命错误信息");
        }

        void Update()
        {
            log.Debug("udp udp udp！！！！！！欢迎");
        }

#if !UNITY_EDITOR
        void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    log.Debug(logString);
                    break;
                case LogType.Warning:
                    log.Warn(logString);
                    break;
                case LogType.Error:
                    log.Error(logString);
                    log.Error(stackTrace);
                    break;
                case LogType.Exception:
                    log.Fatal(logString);
                    log.Fatal(stackTrace);
                    break;
                default:
                    break;
            }
        }
#endif

        public static void Debug(object msg)
        {
            mInstance.log.Debug(msg);
        }

        public static void Info(object msg)
        {
            mInstance.log.Info(msg);
        }

        public static void Warn(object msg)
        {
            mInstance.log.Warn(msg);
        }

        public static void Error(object msg)
        {
            mInstance.log.Error(msg);
        }

        public static void Fatal(object msg)
        {
            mInstance.log.Fatal(msg);
        }

    }
}