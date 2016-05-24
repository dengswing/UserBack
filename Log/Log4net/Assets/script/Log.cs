using log4net;
using log4net.Config;
using System;
using System.IO;
using UnityEngine;
using ILogger = log4net.Core.ILogger;

#pragma warning disable 0414

namespace SimpleFramework
{

    public static class Log
    {
        private static string mCfgName = "log4net";

        static Log()
        {
            byte[] xml = (Resources.Load(mCfgName, typeof(TextAsset)) as TextAsset).bytes;
            XmlConfigurator.Configure(new MemoryStream(xml));
        }

        public static ILog GetLog<T>()
        {
#if UNITY_EDITOR
            LocalLog log = new LocalLog(typeof(T));
            log.Enabled = true;
            return log;
#else
            return LogManager.GetLogger(typeof(T));
#endif
        }
    }


    public class LocalLog : ILog
    {
        private Type mType;
        public LocalLog(Type type)
        {
            mType = type;
        }

        void Debug(object message, Exception exception)
        {
            UnityEngine.Debug.Log("Message:" + message + ",Exception:" + exception);
        }

        public void Debug(object message)
        {
            if (!Enabled)
            {
                return;
            }
            UnityEngine.Debug.Log(mType.Name + " [Debug] " + message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(object message)
        {
            if (!Enabled)
            {
                return;
            }
            UnityEngine.Debug.LogError(mType.Name + " [Error] " + message);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message)
        {
            if (!Enabled)
            {
                return;
            }
            UnityEngine.Debug.LogError(mType.Name + " [Fatal] " + message);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }


        public void Warn(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message)
        {
            if (!Enabled)
            {
                return;
            }
            UnityEngine.Debug.LogWarning(mType.Name + " [Warn] " + message);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(object message, Exception exception)
        {

        }

        public void Info(object message)
        {
            if (!Enabled)
            {
                return;
            }
            UnityEngine.Debug.Log(mType.Name + " [Info] " + message);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool mEnabled;
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }
        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public ILogger Logger
        {
            get
            {
                //if (_logger == null)
                //{
                //    _logger = new ;
                //}
                return null;
            }
        }

        void ILog.Debug(object message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
