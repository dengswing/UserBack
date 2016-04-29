using Networks.interfaces;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using Utilities;

namespace Networks.log
{
    /// <summary>
    /// debug
    /// </summary>
    public class DebugConsole : SingleInstance<DebugConsole>, IDebugConsole
    {
        int sDebugStringMaxLength = 65535 / 4;
        float scrollBarSize = 20;
        Vector2 scrollpos = new Vector2();
        GUIStyle logStyle = null;

        string mDumpDebugFile;
        const string sDumpDebugFileName = "debugDumFile.txt";

        StringBuilder mDumpString = new StringBuilder();
        StringBuilder mDebugString = new StringBuilder();

        /// <summary>
        /// 显示log
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            Log(msg, true);
        }
        public void Log(string msg, bool isTime)
        {
            LogDispose(msg, isTime);
        }

        void LogDispose(string msg, bool isTime)
        {
            if (string.IsNullOrEmpty(msg)) return;

            if (msg.IndexOf("[") != -1 || msg.IndexOf("{") != -1)
            {
                JSONObject j = new JSONObject(msg);
                var tmp = j.Print(true);
                if (tmp != "null") msg = tmp;
            }

            var debug = msg;
            if (mDebugString.Capacity < mDebugString.Length + debug.Length + 50)
            {
                debug = debug.Substring(0, mDebugString.Capacity - mDebugString.Length - 50) + "\n........";
            }

            if (isTime) mDebugString.AppendFormat("======{0}======\n", DateTime.Now.ToString());
            mDebugString.AppendLine(debug);

            if (isTime) mDumpString.AppendFormat("======{0}======\n", DateTime.Now.ToString());
            mDumpString.AppendLine(msg);

            DumpDebugInfoToFile();

        }

        void Awake()
        {
            mDebugString.Capacity = sDebugStringMaxLength;
        }

        #region gui面板

        void LogUI()
        {
            if (GUI.Button(new Rect(0, 10, 100, 50), "open log file"))
            {
                OpenLogFile();
            }

            if (GUI.Button(new Rect(110, 10, 100, 50), "clear log"))
            {
                ClearLog();
            }

            if (GUI.Button(new Rect(220, 10, 100, 50), "clear log file"))
            {
                DelLogFile();
            }

            if (GUI.Button(new Rect(330, 10, 100, 50), "OnDestroy"))
            {
                Destroy(this);
            }

            GUI.BeginGroup(new Rect(0, 60, Screen.width - 10, Screen.height / 2));

#if UNITY_IPHONE
		switch(UnityEngine.iOS.Device.generation)
		{
		case UnityEngine.iOS.DeviceGeneration.iPhone:
		case UnityEngine.iOS.DeviceGeneration.iPhone3G:
		case UnityEngine.iOS.DeviceGeneration.iPhone3GS:
		case UnityEngine.iOS.DeviceGeneration.iPhone4:
		case UnityEngine.iOS.DeviceGeneration.iPhone4S:
		case UnityEngine.iOS.DeviceGeneration.iPhone5:
		case UnityEngine.iOS.DeviceGeneration.iPhoneUnknown:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouchUnknown:
			scrollBarSize = Screen.width/8;
			break;
		case UnityEngine.iOS.DeviceGeneration.iPad1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad3Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad4Gen:
		case UnityEngine.iOS.DeviceGeneration.iPadMini1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPadUnknown:
			scrollBarSize = Screen.width/10;
			break;
		default:
			break;
		}
#elif UNITY_ANDROID
		scrollBarSize = Screen.width/8;
#endif

            GUI.skin.verticalScrollbarThumb.fixedWidth = scrollBarSize;
            GUI.skin.verticalScrollbar.fixedWidth = scrollBarSize;

            scrollpos = GUILayout.BeginScrollView(scrollpos, GUILayout.Width(Screen.width - 10), GUILayout.Height(Screen.height / 2));

            GUILayout.Label(mDebugString.ToString(), logStyle);

            GUILayout.EndScrollView();
            GUI.EndGroup();
        }

        #endregion

        void OnGUI()
        {
            logStyle = GUI.skin.textArea;
            logStyle.fontSize = 30;
            logStyle.fontStyle = FontStyle.Bold;
            LogUI();
        }


        /// <summary>
        /// 清除log
        /// </summary>
        void ClearLog()
        {
            mDebugString.Remove(0, mDebugString.Length);
        }

        /// <summary>
        /// 保存log
        /// </summary>
        void DumpDebugInfoToFile()
        {
            AppendText(mDumpString.ToString());
            mDumpString.Remove(0, mDumpString.Length);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="value"></param>
        void AppendText(string value)
        {
            if (mDumpDebugFile == null)
            {
                mDumpDebugFile = string.Format("{0}/{1}", Application.persistentDataPath, sDumpDebugFileName);
            }

            try
            {
                using (StreamWriter writer = File.AppendText(mDumpDebugFile))
                {
                    writer.Write(value);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        /// <summary>
        /// 打开log
        /// </summary>
        void OpenLogFile()
        {
            if (!string.IsNullOrEmpty(mDumpDebugFile) && File.Exists(mDumpDebugFile)) System.Diagnostics.Process.Start(mDumpDebugFile);
        }

        /// <summary>
        /// 删除log文件
        /// </summary>
        void DelLogFile()
        {
            if (!string.IsNullOrEmpty(mDumpDebugFile) && File.Exists(mDumpDebugFile))
                File.Delete(mDumpDebugFile);
        }

    }
}