using StructGenerate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 类结构生成
/// </summary>
public class ClassStructWindow : EditorWindow
{
    const string sNamesapce = "Vo";

    string csharp = "generated results and error reminder";
    string sMdPath = "ClassStructGenerate";
    string sFilePath = "ClassStructGenerate/Vo/";
    string sPhpPath = "ClassStructGenerate/orm.config.php";
    string sLog = "ClassStructGenerate/Vo/genLog.txt";

    List<string> mdList;
    int index = 0;
    string[] mdName = new string[100];
    GenerateManager generateManager;

    
    [MenuItem("Shinezone/ClassStructWindow %#W")]
    public static void ShowStructWindow()
    {
        EditorWindow.GetWindow(typeof(ClassStructWindow));
    }

    /// <summary>
    /// 类结构生成
    /// </summary>
    public ClassStructWindow() 
    {
        generateManager = new GenerateManager();

        var assetPath = Application.dataPath;
        sMdPath = Path.Combine(assetPath, sMdPath);
        sFilePath = Path.Combine(assetPath, sFilePath);
        sPhpPath = Path.Combine(assetPath, sPhpPath);
        sLog = Path.Combine(assetPath, sLog);
    }

    /// <summary>
    /// 界面绘制
    /// </summary>
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 280));
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        try
        {
            index = EditorGUILayout.Popup(index, mdName);
        }
        catch (System.Exception)
        {

        }

        GUILayout.EndVertical();

        if (GUILayout.Button("GenAllStruct"))
        {
            GenAllStruct();
        }

        if (GUILayout.Button("GenSingleStruct"))
        {
            GenSingleStruct();
        }

        if (GUILayout.Button("Clear Log"))
        {
            ErrorLog.Clear();
            csharp = "";
        }

        if (GUILayout.Button("Open Log"))
        {
            if (File.Exists(sLog)) System.Diagnostics.Process.Start(sLog);
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.TextField(csharp, GUILayout.Width(500), GUILayout.Height(250));

        GUILayout.EndArea();
    }

    /// <summary>
    /// 读取所有*.md文件
    /// </summary>
    void Awake()
    {
        // get md files
        mdList = Directory.GetFiles(sMdPath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(file => file.ToLower().EndsWith(".md"))
            .ToList();

        var length = (mdList != null ? mdList.Count : 0);
        for (int i = 0; i < length; i++)
        {
            mdName[i] = Path.GetFileName(mdList[i]);
        }
    }

    /// <summary>
    /// 生成所有md文件类结构
    /// </summary>
    void GenAllStruct()
    {
        generateManager.MdClassGenerate(mdList, sFilePath, sPhpPath, sNamesapce);
        csharp = ErrorLog.logInfo;

        AssetDatabase.Refresh();

        LogSave();
    }

    /// <summary>
    ///单个md文件类生成 
    /// </summary>
    void GenSingleStruct()
    {
        var singleList = new List<string>();
        singleList.Add(mdList[index]);
 
        generateManager.MdClassGenerate(singleList, sFilePath, sPhpPath, sNamesapce);

        csharp = ErrorLog.logInfo;

        LogSave();
    }

    /// <summary>
    /// log文件生成
    /// </summary>
    void LogSave()
    {
        StreamWriter sw = new StreamWriter(sLog, false, Encoding.UTF8);   
        sw.Write(csharp);
        sw.Flush();
        sw.Close();

        AssetDatabase.Refresh();
    }
}
