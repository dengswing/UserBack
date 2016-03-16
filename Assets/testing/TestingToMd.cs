using StructGenerate;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamasoft.JsonClassGenerator;
using System.IO;
using UnityEditor;

public class TestingToMd : MonoBehaviour
{

    void Start()
    {
        string sFilePath = Path.Combine(Application.dataPath, "Runtime/Vo/");
        string sMdPath = Path.Combine(Application.dataPath, "Resources/test.md");

        var myReadMD = new ReaderMD();
        var structTable = myReadMD.ReadMdToStructTable(sMdPath);

        //Debug.Log(structTable);
        ChangeStructToJson(structTable);

        var myGenerateStruct = new GenerateStruct();
        myGenerateStruct.GenerateTypeStruct(sFilePath, structTable);
    }

    void ChangeStructToJson(List<StructTable> tableData) 
    {
        foreach (var table in tableData) 
        {
           table.jsonTableData = JsonConvert.SerializeObject(table.tableField);
        }
    }
}