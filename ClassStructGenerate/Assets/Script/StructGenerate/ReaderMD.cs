using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace StructGenerate
{
    class ReadConst
    {
        public const string tableName_CN = "表名：";
        public const string tableName_EN = "表名:";

        public const string tableNameConf_CN = "配置表结构：";
        public const string tableNameConf_EN = "配置表结构:";

        public const char poundKey = '#';
        public const char numberKey = '|';
        public const char colonKey = ':';
        public const char braceKey = '{';
        public const char bracketKey = '[';

        public const string fieldInt = "int";
        public const string fieldString = "string";
        public const string fieldJson = "json";
        public const string LineCode = "<code>";
        public const string LineNewLine = "\n";
        public const string LineTabs = "\t";
        public const string LineSpace = " ";
        public const string LineBlank = "";

        public const string splitTable = "'table'=>";
        public const string singleQuotes = "'";
    }

    class StructTable
    {
        public bool isFindTableName = true;
        public string tableName;
        public Dictionary<string, object> tableField;
        public string jsonTableData;
    }

    /// <summary>
    /// 读取md文件
    /// </summary>
    internal class ReaderMD
    {
        string sMdName;

        /// <summary>
        /// 读取及解析md文件
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public List<StructTable> ReadMdToStructTable(string sPath)
        {
            return ReadAndParseMd(sPath);
        }

        /// <summary>
        /// 读取及解析md
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        List<StructTable> ReadAndParseMd(string sPath)
        {
            sMdName = System.IO.Path.GetFileNameWithoutExtension(sPath).Trim();

            var _rstream = new StreamReader(sPath, System.Text.Encoding.UTF8);
            string allContent = _rstream.ReadToEnd();
            _rstream.Close();

            string[] allValue = allContent.Split(new char[2] { ReadConst.poundKey, ReadConst.poundKey });

            var tableList = new List<StructTable>();

            foreach (var readText in allValue)
            {
                if (String.IsNullOrEmpty(readText)) continue;
                var index = FindValue.IndexOfTableName(readText);
                var conIndex = FindValue.IndexOfTableNameConfig(readText);
                if (index == -1 && conIndex == -1) continue;

                var tableData = ReaderLineValue(readText); //解析带表名的数据
                if (tableData != null) tableList.Add(tableData);
            }

            return tableList;
        }

        /// <summary>
        /// 逐行解析
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        StructTable ReaderLineValue(string sValue)
        {
            StringReader reader = new StringReader(sValue);
            string readText = reader.ReadLine();
            var table = new StructTable();
            table.tableField = new Dictionary<string, object>();

            while (readText != null)
            {
                readText = reader.ReadLine();

                if (String.IsNullOrEmpty(readText) || readText == ReadConst.LineCode || readText == ReadConst.LineNewLine) continue; //code标识跳过、\n分割符

                var index = FindValue.IndexOfTableName(readText);
                var conIndex = (index == -1 ? FindValue.IndexOfTableNameConfig(readText) : -1);
                if (index != -1 || conIndex != -1)
                { //查找表名
                    table.isFindTableName = (index != -1);
                    if (table.isFindTableName)
                        table.tableName = readText.Substring(index + ReadConst.tableName_EN.Length).Trim();
                    else
                        table.tableName = readText.Substring(conIndex + ReadConst.tableNameConf_EN.Length).Trim();

                    if (!table.isFindTableName && string.IsNullOrEmpty(table.tableName))
                    { //不需要生成
                        table = null;
                        break;
                    }

                    if (table.tableName == null)
                    {
                        ErrorLog.ShowLogError("{0}.md file [{1}] table data error", true, sMdName, readText);
                    }
                    else
                    { //跳过多余表头
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                    }
                }
                else if (FindValue.IndexOfFieldNumber(readText))
                { //有|分割符的表字段
                    var tableField = ReaderTableField(readText, table.tableName);

                    foreach (var field in tableField)
                    {
                        table.tableField.Add(field.Key, field.Value);
                    }
                }
                else
                { //查找json具体内容
                    WriteTableFieldValue(readText, table, reader);
                }
            }

            return table;
        }

        /// <summary>
        /// 表字段解析
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="sTableName"></param>
        /// <returns></returns>
        Dictionary<string, object> ReaderTableField(string sData, string sTableName)
        {
            string[] aValue = sData.Split(new char[1] { ReadConst.numberKey });
            if (aValue.Length < 2) return null;

            Dictionary<string, object> field = new Dictionary<string, object>();
            string sType = (string)aValue[1].ToLower().Trim();
            string sValue = (string)aValue[0].Trim();
            switch (sType)
            {
                case ReadConst.fieldInt:
                    field.Add(sValue, 1);
                    break;
                case ReadConst.fieldString:
                    field.Add(sValue, "1");
                    break;
                case ReadConst.fieldJson:
                    string sJson = aValue[aValue.Length - 1];

                    var index = sJson.IndexOf(ReadConst.braceKey);
                    if (index == -1) index = sJson.IndexOf(ReadConst.bracketKey);
                    if (index != -1)
                    {
                        sJson = sJson.Substring(index);
                        if (string.IsNullOrEmpty(sJson))
                        {
                            ErrorLog.ShowLogError("{0}.md file [{1}] table [{2}] field data error [{3}]", true, sMdName, sTableName, sValue, aValue[aValue.Length - 1]);
                        }
                        else
                        {
                            var oJson = ChangeJsonObject(sJson, sTableName, sValue); //赋值
                            field.Add(sValue, oJson);
                        }
                    }
                    else
                    {
                        index = sJson.IndexOf(ReadConst.colonKey);
                        if (index != -1)
                            sJson = sJson.Substring(index + 1);
                        else
                            sJson = null;

                        if (string.IsNullOrEmpty(sJson))
                        {
                            ErrorLog.ShowLogError("{0}.md file [{1}] table [{2}] field data error [{3}]", true, sMdName, sTableName, sValue, aValue[aValue.Length - 1]);
                        }

                        field.Add(sValue, sJson);
                    }
                    break;
                default:
                    break;
            }

            return field;
        }

        /// <summary>
        /// 表格字段赋值
        /// </summary>
        /// <param name="sValue"></param>
        /// <param name="table"></param>
        /// <param name="reader"></param>
        void WriteTableFieldValue(string sValue, StructTable table, StringReader reader)
        {
            Dictionary<string, object> tableField = table.tableField;
            string sJson = ReadConst.LineBlank;
            foreach (var field in tableField)
            {
                var oTmp = field.Value;
                string sTmp;
                if (oTmp is string)
                    sTmp = (string)oTmp;
                else
                    continue;

                if (sTmp.Trim().ToLower() == sValue.Trim().ToLower())
                {
                    reader.ReadLine(); //换一行

                    string sFiledValue = reader.ReadLine();
                    while (sFiledValue != ReadConst.LineNewLine && !String.IsNullOrEmpty(sFiledValue))
                    {
                        sJson += sFiledValue;
                        sFiledValue = reader.ReadLine();
                    }

                    tableField[field.Key] = ChangeJsonObject(sJson, table.tableName, field.Key); //赋值
                    break;
                }
            }
        }

        /// <summary>
        /// 内容json化
        /// </summary>
        /// <param name="sJson"></param>
        /// <param name="sTableName"></param>
        /// <param name="sField"></param>
        /// <returns></returns>
        object ChangeJsonObject(string sJson, string sTableName, string sField)
        {
            if (String.IsNullOrEmpty(sJson))
                return null;

            sJson = sJson.Replace(ReadConst.LineTabs, ReadConst.LineBlank).Replace(ReadConst.LineSpace, ReadConst.LineBlank);

            try
            {
                var oJson = JsonConvert.DeserializeObject(sJson);
                return oJson;
            }
            catch (Exception)
            {
                ErrorLog.ShowLogError("{0}.md file [{1}] table [{2}] field json data error [{3}]", true, sMdName, sTableName, sField, sJson);
                return "Error";
            }
        }
    }

    class FindValue
    {
        public static int IndexOfTableName(string sValue)
        {
            var index = sValue.IndexOf(ReadConst.tableName_CN);
            if (index == -1) index = sValue.IndexOf(ReadConst.tableName_EN);
            return index;
        }

        public static int IndexOfTableNameConfig(string sValue)
        {
            var index = sValue.IndexOf(ReadConst.tableNameConf_CN);
            if (index == -1) index = sValue.IndexOf(ReadConst.tableNameConf_EN);
            return index;
        }

        public static bool IndexOfFieldConlon(string sValue)
        {
            return (sValue.IndexOf(ReadConst.colonKey) != -1);
        }

        public static bool IndexOfFieldNumber(string sValue)
        {
            return (sValue.IndexOf(ReadConst.numberKey) != -1);
        }
    }
}