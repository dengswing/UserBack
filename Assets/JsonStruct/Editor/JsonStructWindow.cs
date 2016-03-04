using UnityEngine;
using System.Collections;
using UnityEditor;
using Xamasoft.JsonClassGenerator;
//using Soul;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


public partial class JsonStructWindow : EditorWindow
{
	const string NAMESPACE = "Soul";

	JsonStructWindowInfo[] structs;
	string[] structNames;
	int index = 0;

	string csharp = "C# codes generated here";

	[MenuItem("Window/Soul/JsonStructWindow %#j")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(JsonStructWindow));
	}

	void OnGUI()
	{
		// window start
		GUILayout.BeginArea(new Rect(0, 0, 500, 500));
		GUILayout.BeginVertical();

		// current json files selector
		this.index = EditorGUILayout.Popup(this.index, this.structNames);

		// GenModuleStruct
		if (GUILayout.Button("GenModuleStruct", GUILayout.Height(30)))
		{
			this.GenModuleStruct();
		}

		// json left , c# codes generated right
//		GUILayout.BeginHorizontal();
//		GUILayout.TextField(this.structs[this.index].Json, GUILayout.Width(250));
		GUILayout.TextField(this.csharp, GUILayout.Width(350), GUILayout.Height(250));
//		GUILayout.EndHorizontal();

		// window end
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	void Awake()
	{
		EditorUtility.DisplayProgressBar("Open json struct window", "", 0f);

		// path
		var assetPath = Application.dataPath;
		var resourcesPath = Path.Combine(assetPath, "Resources");
		var jsonsPath = Path.Combine(resourcesPath, "JsonTable");
		
		// get json files
		var jsonFiles = Directory.GetFiles(jsonsPath, "*.*", SearchOption.TopDirectoryOnly)
			.Where(file => file.ToLower().EndsWith(".json") && Regex.Match(Path.GetFileNameWithoutExtension(file), "^sg_.*").Success)
			.ToList();

		// init structs
		this.structs = new JsonStructWindowInfo[jsonFiles.Count];
		this.structNames = new string[jsonFiles.Count];
		for (int i = 0, max = jsonFiles.Count; i < max; i++)
		{
			var jsonFile = jsonFiles[i];
			var JsonStructWindowInfo = new JsonStructWindowInfo(jsonFile);
			this.structs[i] = JsonStructWindowInfo;
			this.structNames[i] = JsonStructWindowInfo.Name;

			EditorUtility.DisplayProgressBar("Open json struct window", "", i / (float)max);
		}

		EditorUtility.ClearProgressBar();
	}
}
partial class JsonStructWindow
{

    static readonly string VO_FILE_PATH = Path.Combine(Application.dataPath, "Runtime/Vo/");
    void GenModuleStruct()
	{
		var JsonStructWindowInfo = this.structs[this.index];

		var gen = new JsonClassGenerator();
        // json text 
        gen.Example = JsonStructWindowInfo.Json;
		// class name
		gen.MainClass = UtilString.TitleCaseString(JsonStructWindowInfo.Name, "_");
		//// name space
		//gen.Namespace = NAMESPACE;
        
        var sw = new StringWriter();
		gen.OutputStream = sw;
		gen.GenerateClasses();
		sw.Flush();
		this.csharp = sw.ToString();
        this.csharp = this.csharp.Replace("IList","List");

        StreamWriter file = new StreamWriter(VO_FILE_PATH + gen.MainClass + ".cs");
        file.Write(this.csharp);
        file.Close();
        AssetDatabase.Refresh();
        Debug.Log(this.csharp);
	}
}
