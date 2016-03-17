using Newtonsoft.Json;
using StructGenerate;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace StructGenerate
{
    /// <summary>
    /// 生成类管理
    /// </summary>
    internal class GenerateManager
    {
        public void MdClassGenerate(List<string> sMdPath, string sFilePath, string sPhpPath)
        {
            if (sMdPath == null || sMdPath.Count <= 0)
            {
                Debug.LogError("md file does not exist");
                return;
            }
            else 
            {
                Debug.Log("md file count=" + sMdPath.Count);
            }

            if (string.IsNullOrEmpty(sPhpPath))
            {
                Debug.LogError("orm.config.php file does not exist");
                return;
            }
            else
            {
                Debug.Log("php file path=" + sPhpPath);
            }

            if (string.IsNullOrEmpty(sFilePath))
            {
                Debug.LogError("generate path does not exist");
                return;
            }
            else 
            {
                Debug.Log("generate file path=" + sFilePath);
            }
            
            var localTime = DateTime.Now;

            //解析php文件
            Dictionary<string, string> tableNameDic;
            if (!ReaderPhp(sPhpPath, out tableNameDic)) return;

            TimeSpan tSpan = DateTime.Now.Subtract(localTime);
            Debug.Log(tableNameDic.Count + " readPhp=>time=" + tSpan.Milliseconds);
            localTime = DateTime.Now;

            //解析md文件
            List<StructTable> structTableList;
            if (!ReaderMd(sMdPath, out structTableList)) return;

            tSpan = DateTime.Now.Subtract(localTime);
            Debug.Log(structTableList.Count + " readMd=>time=" + tSpan.Milliseconds);
            localTime = DateTime.Now;

            //转换json数据及表名获取
            List<StructTable> tableGenerateList;
            if (!ChangeStructToJson(structTableList, tableNameDic, out tableGenerateList)) return;

            tSpan = DateTime.Now.Subtract(localTime);
            Debug.Log(tableGenerateList.Count + " changeJsonAndTableName=>time=" + tSpan.Milliseconds);
            localTime = DateTime.Now;

            //生成class
            var myGenerateStruct = new GenerateStruct();
            myGenerateStruct.GenerateTypeStruct(sFilePath, tableGenerateList);

            tSpan = DateTime.Now.Subtract(localTime);
            Debug.Log("GenerateClass=>time=" + tSpan.Milliseconds);
        }

        bool ReaderMd(List<string> pathList, out List<StructTable> sturctTableList)
        {
            var myReadMD = new ReaderMD();

            sturctTableList = new List<StructTable>();
            foreach (var item in pathList)
            {
                var structTable = myReadMD.ReadMdToStructTable(item);

                if (structTable.Count <= 0)
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(item);
                    Debug.LogFormat("{0}.md file is not found in table", fileName);
                    continue;
                }

                sturctTableList.AddRange(structTable);
            }

            if (sturctTableList.Count <= 0)
            {
                Debug.LogError("md file does not exist");
                return false;
            }

            return true;
        }

        bool ReaderPhp(string sPath, out Dictionary<string, string> tableName)
        {
            var myReaderPhp = new ReaderPhp();
            tableName = myReaderPhp.ReadPhpToStructTable(sPath);

            if (tableName == null || tableName.Count <= 0)
            {
                Debug.LogError("All the tables do not exist");
                return false;
            }

            return true;
        }

        bool ChangeStructToJson(List<StructTable> tableDataList, Dictionary<string, string> tableNameDic, out List<StructTable> tableGenerateList)
        {
            tableGenerateList = new List<StructTable>();
            foreach (var table in tableDataList)
            {
                if (tableNameDic.ContainsKey(table.tableName))
                {
                    try
                    {
                        table.jsonTableData = JsonConvert.SerializeObject(table.tableField);
                    }
                    catch (Exception e)
                    {
                        Debug.LogFormat("{0} serialize json error", table.tableName);
                        continue;
                    }

                    table.tableName = tableNameDic[table.tableName];
                }
                else
                {
                    Debug.LogFormat("{0} Table does not exist", table.tableName);
                    continue;
                }

                tableGenerateList.Add(table);
            }

            if (tableGenerateList.Count <= 0)
            {
                Debug.LogError("All md file find tablename error");
                return false;
            }

            return true;
        }
    }

}