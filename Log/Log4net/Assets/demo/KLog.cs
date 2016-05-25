#define LOCAL

using UnityEngine;
using System.Collections;
using log4net.Config;
using log4net;
using System;
using System.IO;
using log4net.Core;
using log4net.Repository.Hierarchy;

#pragma warning disable 0414

public static class KLog
{
    private static string _fileName =
#if UNITY_ANDROID
        "Config/log4net";
#elif UNITY_STANDALONE_WIN
 Application.dataPath + "/Debug/Log/log4net.xml";
#endif

    static KLog()
    {
#if LOCAL
#else
#if UNITY_ANDROID
        byte[] xml = (Resources.Load(_fileName, typeof(TextAsset)) as TextAsset).bytes;
        XmlConfigurator.Configure(new MemoryStream(xml));
#elif UNITY_STANDALONE_WIN
        XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(_fileName));
#endif
#endif
    }

    public static ILog GetLog<T>()
    {
#if LOCAL
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
    private Type _type;
    public LocalLog(Type type)
    {
        _type = type;
    }

     void Debug(object message, Exception exception)
    {
        UnityEngine.Debug.Log("Message:" + message +",Exception:" + exception);
    }

    public void Debug(object message)
    {
        if (!Enabled)
        {
            return;
        }
        UnityEngine.Debug.Log( _type.Name +" [Debug] " + message);
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
        UnityEngine.Debug.Log( _type.Name +" [Error] " + message);
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
        UnityEngine.Debug.Log(_type.Name + " [Fatal] " + message);
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

    public void Info(object message, Exception exception)
    {
        
    }

    public void Info(object message)
    {
        if (!Enabled)
        {
            return;
        }
        UnityEngine.Debug.Log(_type.Name + " [Info] " + message);
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

    public bool _enabled;
    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }
    public bool IsDebugEnabled
    {
        get {return true; }
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

    public void Warn(object message, Exception exception)
    {
        throw new NotImplementedException();
    }

    public void Warn(object message)
    {
        UnityEngine.Debug.Log("Warn:" + message);
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

    public log4net.Core.ILogger Logger
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