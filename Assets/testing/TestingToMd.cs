using StructGenerate;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamasoft.JsonClassGenerator;
using System.IO;
using UnityEditor;

public class TestingToMd : MonoBehaviour
{
    ReaderMD myReadMD;
    string VO_FILE_PATH;

    void Start()
    {
        VO_FILE_PATH = Path.Combine(Application.dataPath, "Runtime/Vo/");
        myReadMD = new ReaderMD();
        var structTable = myReadMD.ReadMdToStructTable(Application.dataPath + "/Resources/test.md");

        //Debug.Log(structTable);
        ChangeStructToJson(structTable);
    }

    void ChangeStructToJson(List<StructTable> tableData) 
    {
        foreach (var table in tableData) 
        {
           table.jsonTableData = JsonConvert.SerializeObject(table.tableField);
        }

        //Debug.Log(tableData);

        foreach (var table in tableData)
        {
            GenModuleStruct(table);
        }

        AssetDatabase.Refresh();
    }

    void GenModuleStruct(StructTable tableData)
    {     
        var gen = new JsonClassGenerator();
        // json text 
        gen.Example = tableData.jsonTableData;
        // class name
        gen.MainClass = tableData.tableName;
        //// name space
        //gen.Namespace = NAMESPACE;

        var sw = new StringWriter();
        gen.OutputStream = sw;
        gen.GenerateClasses();
        sw.Flush();
        var csharp = sw.ToString();
        csharp = csharp.Replace("IList", "List");

        StreamWriter file = new StreamWriter(VO_FILE_PATH + gen.MainClass + ".cs");
        file.Write(csharp);
        file.Close();
       // Debug.Log(csharp);
    }
}