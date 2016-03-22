using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StructGenerate
{

    /// <summary>
    /// 生成表结构列表
    /// </summary>
    public class GenerateTableList
    {
        const string tableStruct = "TableDataStruct";
        const string tableNames = "TableDataNames";


        public void GenClassesStructFile(string namespaces, string sPath, string namespacesClass)
        {
            var mdList = Directory.GetFiles(sPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(file => file.ToLower().EndsWith(".cs"))
                .ToList();

            var length = (mdList != null ? mdList.Count : 0);
            List<string> mdName = new List<string>();
            for (int i = 0; i < length; i++)
            {
                var name = Path.GetFileNameWithoutExtension(mdList[i]);
                if (name != tableStruct && name != tableNames)
                    mdName.Add(name);
            }

            if (mdName.Count > 0)
                WriteClassesToFile(namespaces, sPath, mdName, namespacesClass);
        }


        private void WriteClassesToFile(string namespaces, string sPath, List<string> classList, string namespacesClass)
        {
            if (string.IsNullOrEmpty(sPath) || classList == null || classList.Count <= 0) return;

            var sOld = sPath + "/old/";
            var sClass = string.Format("{0}/data", sPath);
            var sStruct = string.Format("{0}/{1}.cs", sClass, tableStruct);
            var sNames = string.Format("{0}/{1}.cs", sClass, tableNames);

            if (!Directory.Exists(sClass))
            {
                Directory.CreateDirectory(sClass);
            }

            GenerateStruct.CreateClassToTxt(sStruct, sOld);
            GenerateStruct.CreateClassToTxt(sNames, sOld);

            var sw = new StringWriter();
            GenTableClass.WriteFileStart(sw);
            if (!string.IsNullOrEmpty(namespaces))
            {
                GenTableClass.WriteNamespaceStart(namespaces, sw, namespacesClass);
            }
            else
            {
                sw.WriteLine("using {0};", namespacesClass);
                sw.WriteLine("using Networks.parser;");
                sw.WriteLine();
            }

            if (!string.IsNullOrEmpty(namespaces))
            {
                GenTableClass.WriteClassTableSpace(tableStruct, sw, classList);
                GenTableClass.WriteNamespaceEnd(sw);
            }
            else
            {
                GenTableClass.WriteClassTable(tableStruct, sw, classList);
            }

            StreamWriter file = new StreamWriter(sStruct);
            file.Write(sw.ToString());
            file.Flush();
            file.Close();
            sw.Dispose();
            sw.Close();

            sw = new StringWriter();
            GenTableClass.WriteFileStart(sw);
            if (!string.IsNullOrEmpty(namespaces)) GenTableClass.WriteNamespaceStart(namespaces, sw, null);


            if (!string.IsNullOrEmpty(namespaces))
            {
                GenTableNames.WriteClassTableSpace(tableNames, sw, classList);
                GenTableClass.WriteNamespaceEnd(sw);
            }
            else
            {
                GenTableNames.WriteClassTable(tableNames, sw, classList);
            }

            file = new StreamWriter(sNames);
            file.Write(sw.ToString());
            file.Flush();
            file.Close();
            sw.Dispose();
            sw.Close();
        }
    }

    class GenTableNames
    {
        public static void WriteClassTableSpace(string assignedName, TextWriter sw, List<string> classList)
        {
            var visibility = "public";

            sw.WriteLine("    {0} class {1}", visibility, assignedName);
            sw.WriteLine("    {");
            foreach (var item in classList)
            {
                sw.WriteLine("        public const string {0} = \"{1}\";", item, item);
            }
            sw.WriteLine("    }");
            sw.WriteLine();
        }

        public static void WriteClassTable(string assignedName, TextWriter sw, List<string> classList)
        {
            var visibility = "public";

            sw.WriteLine("{0} class {1}", visibility, assignedName);
            sw.WriteLine("{");
            foreach (var item in classList)
            {
                sw.WriteLine("    public const string {0} = \"{1}\";", item, item);
            }
            sw.WriteLine("}");
            sw.WriteLine();
        }
    }

    class GenTableClass
    {
        public static void WriteFileStart(TextWriter sw)
        {
            sw.WriteLine("//According to the Json file automatically generated structures");
            sw.WriteLine("//Date : " + DateTime.Now.ToString());
            sw.WriteLine();
        }

        public static void WriteNamespaceStart(string namespaces, TextWriter sw, string namespacesClass)
        {
            if (!string.IsNullOrEmpty(namespacesClass) && namespacesClass != namespaces)
            {
                sw.WriteLine("using {0};", namespacesClass);
                if ("Networks.parser" != namespaces)
                    sw.WriteLine("using Networks.parser;");
                sw.WriteLine();
            }

            sw.WriteLine("namespace {0}", namespaces);
            sw.WriteLine("{");
            sw.WriteLine();
        }

        public static void WriteNamespaceEnd(TextWriter sw)
        {
            sw.WriteLine("}");
        }

        public static void WriteClassTableSpace(string assignedName, TextWriter sw, List<string> classList)
        {
            var visibility = "public";

            sw.WriteLine("    {0} class {1} : AbsTableDataStruct", visibility, assignedName);
            sw.WriteLine("    {");
            sw.WriteLine();

            sw.WriteLine("        Networks.DataTableUpdateDelegate generalResponse;");
            sw.WriteLine();

            sw.WriteLine("        public override void RegisterBindingTableStrcut()");
            sw.WriteLine("        {");

            foreach (var item in classList)
            {
                sw.WriteLine("            typeDict[TableDataNames.{0}] = typeof({1});", item, item);
            }

            sw.WriteLine("        }");
            sw.WriteLine();

            sw.WriteLine("        public void AddGeneralResponse(Networks.DataTableUpdateDelegate response)");
            sw.WriteLine("        {");
            sw.WriteLine("            generalResponse -= response;");
            sw.WriteLine("            generalResponse += response;");
            sw.WriteLine("        }");
            sw.WriteLine();

            sw.WriteLine("        public void RemoveGeneralResponse(Networks.DataTableUpdateDelegate response)");
            sw.WriteLine("        {");
            sw.WriteLine("            generalResponse -= response;");
            sw.WriteLine("        }");
            sw.WriteLine();

            sw.WriteLine("        public override void FireNotice(string tableName, object data)");
            sw.WriteLine("        {");
            sw.WriteLine("            if (generalResponse != null)");
            sw.WriteLine("            {");
            sw.WriteLine("                generalResponse(data);");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine();

            sw.WriteLine("    }");
            sw.WriteLine();
        }

        public static void WriteClassTable(string assignedName, TextWriter sw, List<string> classList)
        {
            var visibility = "public";

            sw.WriteLine("{0} class {1} : AbsTableDataStruct", visibility, assignedName);
            sw.WriteLine("{");
            sw.WriteLine();

            sw.WriteLine("    Networks.DataTableUpdateDelegate generalResponse;");
            sw.WriteLine();

            sw.WriteLine("    public override void RegisterBindingTableStrcut()");
            sw.WriteLine("    {");

            foreach (var item in classList)
            {
                sw.WriteLine("        typeDict[TableDataNames.{0}] = typeof({1});", item, item);
            }

            sw.WriteLine("    }");
            sw.WriteLine();

            sw.WriteLine("    public void AddGeneralResponse(Networks.DataTableUpdateDelegate response)");
            sw.WriteLine("    {");
            sw.WriteLine("        generalResponse -= response;");
            sw.WriteLine("        generalResponse += response;");
            sw.WriteLine("    }");
            sw.WriteLine();

            sw.WriteLine("    public void RemoveGeneralResponse(Networks.DataTableUpdateDelegate response)");
            sw.WriteLine("    {");
            sw.WriteLine("        generalResponse -= response;");
            sw.WriteLine("    }");
            sw.WriteLine();

            sw.WriteLine("    public override void FireNotice(string tableName, object data)");
            sw.WriteLine("    {");
            sw.WriteLine("        if (generalResponse != null)");
            sw.WriteLine("        {");
            sw.WriteLine("            generalResponse(data);");
            sw.WriteLine("        }");
            sw.WriteLine("    }");
            sw.WriteLine();

            sw.WriteLine("}");
            sw.WriteLine();
        }


    }
}