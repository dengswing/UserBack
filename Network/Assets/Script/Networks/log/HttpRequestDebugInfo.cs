//#define SCROLLTEST
#define USE_REQUEST_DEBUG

using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;

public class HttpRequestDebugInfo : MonoBehaviour
{
	private enum EDebugType
	{
		[Description("Debug Request")]	EDebugRequest,
		[Description("Debug Other")]	EDebugString,
	}

    enum EDebugLevel
    {
        [Description("Basic Log Info")]
        EDebugLevelOne,	// only url , time and size
        [Description("Log Info With Headers")]
        EDebugLevelTwo, // add headers
        [Description("Full Log Info")]
        EDebugLevelThree, // add contents(body)
    }

	private EDebugType mDebugType = EDebugType.EDebugRequest;
	
	//	private string mOutputFile = "";
	//	private string responseString = "";
	private StringBuilder mOtherDebugString = new StringBuilder(); // for other debug
	private string mOtherDumpFileName = "otherDebugDump.txt";
	private string mOtherDumpFile = null;
	
	private Vector2 scrollpos = new Vector2();
	private float scrollBarSize = 40;
	private GUIStyle logStyle = null;
	
	//	private enum EDebugInfoState
	//	{
	//		ESwapping,
	//		ESwapped,
	//	}
	
	private EDebugLevel mDebugLevel = EDebugLevel.EDebugLevelOne;
	//	private EDebugInfoState mDebugInfoState = EDebugInfoState.ESwapped;
	private int mDebugIndex = 0;
	private int sDebugBufferLength = 20; // length for eache List
	//private List<List<HttpRequest.DebugInfo>> mDebugInfos = new List<List<HttpRequest.DebugInfo>>();
	private StringBuilder mDebugString =  new StringBuilder(); 
	private StringBuilder mDumpString = new StringBuilder();
	private static string sDumpDebugFileName = "debugDumFile.txt";
	private string mDumpDebugFile = null;
	
	private static string sDebugBreakLine = "@@@@@@@@@@@@@@@@@@@@\n";
	// label maxVertices < 65536 so maxLength is 16383
	private static int sDebugStringMaxLength = 65535 / 4;

	private int mRequestCount = 0;
	private long mRequestSize = 0;
	private long mResponseSize = 0;

	void Awake ()
	{
#if USE_REQUEST_DEBUG
		//		string path = Application.persistentDataPath;
		if(mDumpDebugFile == null)
		{
			mDumpDebugFile = string.Format("{0}/{1}", Application.persistentDataPath, sDumpDebugFileName);
			File.CreateText(mDumpDebugFile).Close();
		}
		
		//mDebugInfos.Add(new List<HttpRequest.DebugInfo>());
		//mDebugInfos.Add(new List<HttpRequest.DebugInfo>());
		
		mDebugString.Capacity = sDebugStringMaxLength;

		mOtherDebugString.Append(mDumpDebugFile);
		mOtherDebugString.Append("\n");

		AddDebugListener();

        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
        ExpendLogContent();
#endif
	}
	
	public void AddDebugListener()
	{
#if USE_OTHER_DEBUG
		MessengerManager.AddListener<object>(HttpRequest.sDebugInfoEventType, OtherDebugInfoResponse);	// listener for other debug
#endif
		//MessengerManager.AddListener<object>(HttpRequest.sDebugInfoEventType2, RequestdebugInfoResponse); // listener for network request
		//MessengerManager.AddListener<long>(HttpRequest.sDebugInfoRequestSize, DebugInfoRequestSize); // listener for accumulate request byte
		//MessengerManager.AddListener<long>(HttpRequest.sDebugInfoResponseSize, DebugInfoResponseSize); // listener for accumulate response byte
	}

	// Use this for initialization
	void Start ()
	{
#if USE_REQUEST_DEBUG
		if (mDumpDebugFile == null)
		{
			mDumpDebugFile = string.Format("{0}/{1}", Application.persistentDataPath, sDumpDebugFileName);
			File.CreateText(mDumpDebugFile).Close();
		}
#endif
	}

	//Vector2 sp = new Vector2(0, 0);
	void OnGUI ()
	{
#if USE_REQUEST_DEBUG
#if SCROLLTEST
		float vW = 100;
		float vH = 100;
		float cW = 300;
		float cH = 300;
		Rect vR = new Rect(0, 0, 200, 200);
		Rect cR = new Rect(0, 100, 300, 400);

		GUI.BeginScrollView(cR, sp, vR, true, true);
		GUILayout.TextArea("character/FengXinZi/Level03dfdfsdfdsfcharacter/FengXinZi/Level03character/FengXinZi/Level03character/FengXinZi/Level03character/FengXinZi/Level03", 20);
		GUI.EndScrollView();

		Rect sbR = new Rect(0, 0, 20, 20);
//		sp.y = GUI.VerticalScrollbar(sbR, sp.y, cR.height, 0, vR.height);
//		sp.x = GUI.VerticalScrollbar(sbR, sp.x, cR.width, 0, vR.width);
#else
		logStyle = GUI.skin.textArea;
		logStyle.fontSize = Screen.height / 50;
		logStyle.fontStyle = FontStyle.Bold;
		LogUI();
		RequestCountUI();
#endif
#endif
	}
	
	private void RequestCountUI()
	{
		int width = Screen.width / 3;
		int height = Screen.height/ 2;
		GUILayout.BeginArea(new Rect(width, 0, width, height));
				GUILayout.TextArea("Debug");
		GUILayout.EndArea();

		GUILayout.BeginArea (new Rect (width + width, 0, width, height));
		GUILayout.Label(string.Format("Request Count : [{0}]", 100), logStyle);
		GUILayout.Label(string.Format("Request Byte: {0}", mRequestSize),logStyle);
		GUILayout.Label(string.Format("Response Byte: {0}", mResponseSize), logStyle);

		if (GUILayout.Button ("Reset Request Count", GUILayout.Height(Screen.height / 15)))
		{
			ResetRequestCount();
		}
		if (GUILayout.Button("DebugLevel",GUILayout.Height(Screen.height / 15)))
		{
			switch(mDebugLevel)
			{
			case EDebugLevel.EDebugLevelOne:
				mDebugLevel = EDebugLevel.EDebugLevelTwo;
				break;
			case EDebugLevel.EDebugLevelTwo:
				mDebugLevel = EDebugLevel.EDebugLevelThree;
				break;
			case EDebugLevel.EDebugLevelThree:
				mDebugLevel = EDebugLevel.EDebugLevelOne;
				break;
			default:
				mDebugLevel = EDebugLevel.EDebugLevelOne;
				break;
			}
			ResetLogContent();
		}
		
		GUILayout.EndArea ();
	}

    //private void FullIllustration()
    //{
    //    IllustrationNewRequest request = IllustrationNewRequest.instance;
    //    List<int> allFairies = new List<int>();

    //    foreach(FairyTemplateInfo info in Fairy.sFairyInfos.Values)
    //    {
    //        if (info == null)
    //            continue;

    //        if (info.bShow == true)
    //            allFairies.Add(info.templateId);
    //    }
    //    request.newList = allFairies;
    //    request.SendRequest();
    //}

	private void LogUI()
	{
		if (GUI.Button(new Rect(0, Screen.height / 2 - 50, 150, 40), "Clear and dump to file"))
		{
			//			OutputFile();
			switch (mDebugType)
			{
			case EDebugType.EDebugRequest:
				DumpDebugInfoToFile();
				break;
			case EDebugType.EDebugString:
				if (mOtherDumpFile == null)
				{
					mOtherDumpFile = string.Format("{0}/{1}", Application.persistentDataPath, mOtherDumpFile);
				}
				try
				{
					using (StreamWriter writer = File.AppendText(mOtherDumpFile))
					{
						writer.Write(mOtherDebugString.ToString());
						writer.Close();
						mOtherDebugString.Remove(0, mOtherDebugString.Length);
					}
				}
				catch (Exception e)
				{
					Debug.LogWarning(e.Message);
				}
				break;
			default:
				break;
			}
		}
		if (GUI.Button(new Rect(200, Screen.height / 2 - 50, 150, 40),"1111"))
		{
			switch(mDebugType)
			{
			case EDebugType.EDebugRequest:
				mDebugType = EDebugType.EDebugString;
				break;
			case EDebugType.EDebugString:
				mDebugType  = EDebugType.EDebugRequest;
				break;
			default:
				break;
			}
		}

		if (GUI.Button(new Rect(400, Screen.height / 2 - 50, 150, 40), "FullIllustration"))
		{
			//GameManager.instance.ResetToLogin();
			//FullIllustration();
		}


		GUI.BeginGroup(new Rect(0, Screen.height / 2, Screen.width - 10, Screen.height / 2));

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

		scrollpos = GUILayout.BeginScrollView(scrollpos,GUILayout.Width(Screen.width - 10), GUILayout.Height(Screen.height / 2));
		//		GUILayout.TextArea(responseString, responseString.Length, logStyle);
		switch(mDebugType)
		{
		case EDebugType.EDebugString:
			GUILayout.Label(mOtherDebugString.ToString(), logStyle);
			break;
		default:
			GUILayout.Label(mDebugString.ToString(), logStyle);
			break;
		}
		GUILayout.EndScrollView();
		GUI.EndGroup();
	}
	
	private void RequestdebugInfoResponse(object param)
	{
		
            //if (debugInfos.Count < sDebugBufferLength)
            //{
				ExpendLogContent();
            //}
            //else
            //{				
            //    DumpDebugInfoToFile();
            //}
		
	}
	
	private void DumpDebugInfoToFile()
	{
        mDumpString.Append(sDebugBreakLine);
		
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
	
	private void ExpendLogContent()
	{
		
		mDebugString.Append(sDebugBreakLine);
        mDebugString.Append("=====>>" + mDebugString.Length);
        mDebugString.AppendLine(string.Format("length[{0}], capacity[{1}], maxCapacity[{2}]", mDebugString.Length, mDebugString.Capacity, mDebugString.MaxCapacity));
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("YYRRTET");
        mDebugString.AppendLine("GGGGGGGGGGGG");
	}
	
	private void ResetLogContent()
	{
		mDebugString.Remove(0, mDebugString.Length);		
	}
	
	private void DisplayDebugInfo()
	{
	}
	
	private void OtherDebugInfoResponse(object para)
	{
		string debugString = para as String;
		if (string.IsNullOrEmpty (debugString))
			return;

		mOtherDebugString.Append(debugString);
	}

	private void DebugInfoRequestSize(long reqeustSize)
	{
		mRequestSize += reqeustSize;
	}

	private void DebugInfoResponseSize(long responseSize)
	{
		mResponseSize += responseSize;
	}

	private void ResetRequestCount()
	{
		mRequestSize = 0;
		mResponseSize = 0;
	}
}

