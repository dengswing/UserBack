using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Xamasoft.JsonClassGenerator;

namespace StructGenerate
{
    /// <summary>
    /// 生成数据结构
    /// </summary>
    internal class GenerateStruct
    {
        public void GenerateTypeStruct(string sPath,List<StructTable> tableList)
        {
            foreach (var table in tableList)
            {
                GenModuleStruct(table, sPath);
            }

            AssetDatabase.Refresh();
        }

        void GenModuleStruct(StructTable tableData, string sPath)
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

            StreamWriter file = new StreamWriter(sPath + gen.MainClass + ".cs");
            file.Write(csharp);
            file.Close();
            // Debug.Log(csharp);
        }
    }

}