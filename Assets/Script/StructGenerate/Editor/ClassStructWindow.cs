using StructGenerate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

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

    public ClassStructWindow() 
    {
        generateManager = new GenerateManager();

        var assetPath = Application.dataPath;
        sMdPath = Path.Combine(assetPath, sMdPath);
        sFilePath = Path.Combine(assetPath, sFilePath);
        sPhpPath = Path.Combine(assetPath, sPhpPath);
        sLog = Path.Combine(assetPath, sLog);
    }


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

    void GenAllStruct()
    {
        //if (generateManager== null) generateManager = new GenerateManager(); 
        generateManager.MdClassGenerate(mdList, sFilePath, sPhpPath, sNamesapce);
        csharp = ErrorLog.logInfo;

        AssetDatabase.Refresh();

        LogSave();
    }

    void GenSingleStruct()
    {
        var singleList = new List<string>();
        singleList.Add(mdList[index]);

        // if (generateManager == null) generateManager = new GenerateManager(); 
        generateManager.MdClassGenerate(singleList, sFilePath, sPhpPath, sNamesapce);

        csharp = ErrorLog.logInfo;

        LogSave();
    }

    void LogSave()
    {
        //FileStream fs = new FileStream(sLog, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(sLog, false, Encoding.UTF8);   
        sw.Write(csharp);
        sw.Flush();
        sw.Close();
        //fs.Close();

        AssetDatabase.Refresh();
    }
}
