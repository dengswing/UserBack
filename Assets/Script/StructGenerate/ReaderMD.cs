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
        public const char poundKey = '#';
        public const char numberKey = '|';
        public const char colonKey = ':';
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
        public string tableName;
        public Dictionary<string, object> tableField;
        public string jsonTableData;
    }

    /// <summary>
    /// 读取md文件
    /// </summary>
    internal class ReaderMD
    {
        public List<StructTable> ReadMdToStructTable(string sPath)
        {
            return ReadAndParseMd(sPath);
        }

        List<StructTable> ReadAndParseMd(string sPath)
        {
            var _rstream = new StreamReader(sPath, System.Text.Encoding.UTF8);
            string allContent = _rstream.ReadToEnd();
            _rstream.Close();

            string[] allValue = allContent.Split(new char[2] { ReadConst.poundKey, ReadConst.poundKey });

            var tableList = new List<StructTable>();

            foreach (var readText in allValue)
            {
                if (String.IsNullOrEmpty(readText)) continue;
                var index = FindValue.IndexOfTableName(readText);
                if (index == -1) continue;

                var tableData = ReaderLineValue(readText); //解析带表名的数据
                tableList.Add(tableData);
            }

            return tableList;
        }

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
                if (index != -1)
                { //查找表名
                    table.tableName = readText.Substring(index + ReadConst.tableName_EN.Length);

                    if (table.tableName == null)
                    {
                        Debug.LogWarning("Table name is null =>" + readText);
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
                    var tableField = ReaderTableField(readText);

                    foreach (var field in tableField)
                    {
                        table.tableField.Add(field.Key, field.Value);
                    }
                }
                else
                { //查找json具体内容
                    WriteTableFieldValue(readText, table.tableField, reader);
                }
            }

            return table;
        }

        Dictionary<string, object> ReaderTableField(string sData)
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
                    var index = sJson.IndexOf(ReadConst.colonKey);
                    if (index != -1)
                        sJson = sJson.Substring(index + 1);
                    else
                        sJson = null;

                    field.Add(sValue, sJson);
                    break;
                default:
                    break;
            }

            return field;
        }

        void WriteTableFieldValue(string sValue, Dictionary<string, object> tableField, StringReader reader)
        {
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

                    tableField[field.Key] = ChangeJsonObject(sJson); //赋值
                    break;
                }
            }
        }

        object ChangeJsonObject(string sJson)
        {
            if (String.IsNullOrEmpty(sJson))
                return null;

            sJson = sJson.Replace(ReadConst.LineTabs, ReadConst.LineBlank).Replace(ReadConst.LineSpace, ReadConst.LineBlank);

            var oJson = JsonConvert.DeserializeObject(sJson);
            return oJson;
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