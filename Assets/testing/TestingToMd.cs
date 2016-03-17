using Newtonsoft.Json;
using StructGenerate;
using System;
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


        var mdList = new List<string>();
        mdList.Add(sMdPath);
        mdList.Add(Path.Combine(Application.dataPath, "ClassStructGenerate/生产系统设计.md"));

        var generateManager = new GenerateManager();
        generateManager.MdClassGenerate(mdList, sFilePath, sPhpPath);
    }   
}