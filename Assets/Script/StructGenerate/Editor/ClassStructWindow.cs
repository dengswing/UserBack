using StructGenerate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ClassStructWindow : EditorWindow
{
    const string sNamesapce = "Vo";

    string csharp = "generated results and error reminder";
    string sMdPath = "ClassStructGenerate";
    string sFilePath = "ClassStructGenerate/Vo/";
    string sPhpPath = "ClassStructGenerate/orm.config.php";

    List<string> mdList;
    int index = 0;
    string[] mdName = new string[100];
    GenerateManager generateManager;

    [MenuItem("Shinezone/ClassStructWindow %#W")]
    public static void ShowStructWindow()
    {
        EditorWindow.GetWindow(typeof(ClassStructWindow));
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

        GUILayout.EndHorizontal();

        GUILayout.TextField(csharp, GUILayout.Width(500), GUILayout.Height(250));

        GUILayout.EndArea();
    }

    void Awake()
    {
        generateManager = new GenerateManager(); 

        var assetPath = Application.dataPath;
        sMdPath = Path.Combine(assetPath, sMdPath);
        sFilePath = Path.Combine(assetPath, sFilePath);
        sPhpPath = Path.Combine(assetPath, sPhpPath);

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
        generateManager.MdClassGenerate(mdList, sFilePath, sPhpPath, sNamesapce);
        csharp = ErrorLog.logInfo;
    }

    void GenSingleStruct()
    {
        var singleList = new List<string>();
        singleList.Add(mdList[index]);
        generateManager.MdClassGenerate(singleList, sFilePath, sPhpPath, sNamesapce);

        csharp = ErrorLog.logInfo;
    }
}
