using Newtonsoft.Json;
using StructGenerate;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestingToMd : MonoBehaviour
{

    void Start()
    {
        string sFilePath = Path.Combine(Application.dataPath, "ClassStructGenerate/Runtime/Vo/");
        string sMdPath = Path.Combine(Application.dataPath, "ClassStructGenerate/test.md");
        string sPhpPath = Path.Combine(Application.dataPath, "ClassStructGenerate/orm.config.php");

        var myReaderPhp = new ReaderPhp();
        var tableName = myReaderPhp.ReadPhpToStructTable(sPhpPath);
        Debug.Log(tableName.Count);

        var myReadMD = new ReaderMD();
        var structTable = myReadMD.ReadMdToStructTable(sMdPath);

        Debug.Log(structTable.Count);
        ChangeStructToJson(structTable, tableName);

        var myGenerateStruct = new GenerateStruct();
        myGenerateStruct.GenerateTypeStruct(sFilePath, structTable);
    }

    void ChangeStructToJson(List<StructTable> tableData, Dictionary<string, string> tableName)
    {
        if (tableName == null || tableName.Count <= 0) Debug.LogError("All the tables do not exist");

        var generateTable = new List<StructTable>();
        foreach (var table in tableData)
        {
            if (tableName.ContainsKey(table.tableName))
            {
                table.jsonTableData = JsonConvert.SerializeObject(table.tableField);
                table.tableName = tableName[table.tableName];
            }
            else
            {
                Debug.LogWarningFormat("{0} Table does not exist", table.tableName);
                continue;
            }

            generateTable.Add(table);
        }
    }
}