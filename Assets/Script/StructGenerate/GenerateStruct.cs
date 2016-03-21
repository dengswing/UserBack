using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Xamasoft.JsonClassGenerator;
using System.Linq;

namespace StructGenerate
{
    /// <summary>
    /// 生成数据结构
    /// </summary>
    internal class GenerateStruct
    {
        /// <summary>
        /// 生成数据结构
        /// </summary>
        /// <param name="sPath">生成类路径</param>
        /// <param name="tableList">表数据</param>
        /// <param name="sNamespace">类命名空间</param>
        public void GenerateTypeStruct(string sPath, List<StructTable> tableList, string sNamespace)
        {
            foreach (var table in tableList)
            {
                GenModuleStruct(table, sPath, sNamespace);
            }
        }

        /// <summary>
        /// 生成结构
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="sPath"></param>
        /// <param name="sNamespace"></param>
        void GenModuleStruct(StructTable tableData, string sPath, string sNamespace)
        {
            var gen = new JsonClassGenerator();

            // json text 
            gen.Example = tableData.jsonTableData;
            // class name
            gen.MainClass = tableData.tableName;
            //// name space
            if (!string.IsNullOrEmpty(sNamespace))
                gen.Namespace = sNamespace;
            gen.UseProperties = true;

            var sw = new StringWriter();
            gen.OutputStream = sw;
            gen.GenerateClasses();
            sw.Flush();
            var csharp = sw.ToString();
            csharp = csharp.Replace("IList", "List");

            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }

            var sOld = sPath + "/old/";
            if (!Directory.Exists(sOld))
            {
                Directory.CreateDirectory(sOld);
            }

            sPath = sPath + gen.MainClass + ".cs";
            if (File.Exists(sPath))
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(sPath);
                var fileDel = sOld + fileName + ".txt";
                if (File.Exists(fileDel))
                {
                    FileInfo fi = new FileInfo(fileDel);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(fileDel);
                }

                File.Copy(sPath, fileDel);
            }

            StreamWriter file = new StreamWriter(sPath);
            file.Write(csharp);
            file.Close();
        }

    }

}