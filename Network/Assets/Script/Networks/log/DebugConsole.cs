using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using UnityEngine;

namespace Networks.log
{
    public class DebugConsole : MonoBehaviour
    {
        enum EDebugType
        {
            [Description("Debug Request")]
            EDebugRequest,
            [Description("Debug Other")]
            EDebugString,
        }

        static GameObject gameContainer = null;
        static DebugConsole _Instance;
        static DebugConsole Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<DebugConsole>();
                    if (_Instance == null && gameContainer == null)
                    {
                        gameContainer = new GameObject();
                        gameContainer.name = "DebugConsole";
                        gameContainer.AddComponent(typeof(DebugConsole));
                    }

                    _Instance = FindObjectOfType<DebugConsole>();
                    if (_Instance != null)
                        DontDestroyOnLoad(_Instance.gameObject);
                }
                return _Instance;
            }
        }

        float scrollBarSize = 20;
        Vector2 scrollpos = new Vector2();
        GUIStyle logStyle = null;

        string mDumpDebugFile;
        const string sDumpDebugFileName = "debugDumFile.txt";

        StringBuilder mDumpString = new StringBuilder();
        StringBuilder mDebugString = new StringBuilder();

        EDebugType mDebugType = EDebugType.EDebugRequest;

        public static void Log(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            Instance.mDebugString.AppendFormat("================={0}=================\n", DateTime.Now.ToString());
            Instance.mDebugString.AppendLine(msg);
            Instance.mDumpString.AppendFormat("\n================={0}=================\n", DateTime.Now.ToString());
            Instance.mDumpString.AppendLine(msg);
            Instance.DumpDebugInfoToFile();
        }

        void LogUI()
        {
            if (GUI.Button(new Rect(0, 10, 150, 40), "open log file"))
            {
                OpenLogFile();
            }

            if (GUI.Button(new Rect(0, 50, 150, 40), "clear log"))
            {
                ClearLog();
            }

            GUI.BeginGroup(new Rect(0, 100, Screen.width - 10, Screen.height / 2));

#if UNITY_IPHONE
		switch(iPhone.generation)
		{
		case iPhoneGeneration.iPhone:
		case iPhoneGeneration.iPhone3G:
		case iPhoneGeneration.iPhone3GS:
		case iPhoneGeneration.iPhone4:
		case iPhoneGeneration.iPhone4S:
		case iPhoneGeneration.iPhone5:
		case iPhoneGeneration.iPhoneUnknown:
		case iPhoneGeneration.iPodTouch1Gen:
		case iPhoneGeneration.iPodTouch2Gen:
		case iPhoneGeneration.iPodTouch3Gen:
		case iPhoneGeneration.iPodTouch4Gen:
		case iPhoneGeneration.iPodTouch5Gen:
		case iPhoneGeneration.iPodTouchUnknown:
			scrollBarSize = Screen.width/8;
			break;
		case iPhoneGeneration.iPad1Gen:
		case iPhoneGeneration.iPad2Gen:
		case iPhoneGeneration.iPad3Gen:
		case iPhoneGeneration.iPad4Gen:
		case iPhoneGeneration.iPadMini1Gen:
		case iPhoneGeneration.iPadUnknown:
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

        void OnGUI()
        {
            logStyle = GUI.skin.textArea;
            logStyle.fontSize = 14;
            logStyle.fontStyle = FontStyle.Bold;
            LogUI();
        }

        void ClearLog()
        {
            mDebugString.Remove(0, mDebugString.Length);
        }

        void DumpDebugInfoToFile()
        {
            if (mDumpDebugFile == null)
            {
                mDumpDebugFile = string.Format("{0}/{1}", Application.persistentDataPath, sDumpDebugFileName);
            }

            try
            {
                using (StreamWriter writer = File.AppendText(mDumpDebugFile))
                {
                    writer.Write(mDumpString.ToString());
                    writer.Close();
                    mDumpString.Remove(0, mDumpString.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        void OpenLogFile()
        {
            if (!string.IsNullOrEmpty(mDumpDebugFile) && File.Exists(mDumpDebugFile)) System.Diagnostics.Process.Start(mDumpDebugFile);
        }

    }
}