﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace StructGenerate
{
    /// <summary>
    /// 生成类管理
    /// </summary>
    public class GenerateManager
    {
        DateTime iTotal;
        ReaderPhp myReaderPhp;
        ReaderMD myReadMD;
        GenerateStruct myGenerateStruct;

        /// <summary>
        /// 生成类管理
        /// </summary>
        public GenerateManager()
        {
            myReaderPhp = new ReaderPhp();
            myReadMD = new ReaderMD();
            myGenerateStruct = new GenerateStruct();
        }

        /// <summary>
        /// md文件类结构生成
        /// </summary>
        /// <param name="sMdPath">所有md文件路径</param>
        /// <param name="sFilePath">生成类文件夹路径</param>
        /// <param name="sPhpPath">php文件路径</param>
        /// <param name="sNamesapce">生成类的命名空间</param>
        public void MdClassGenerate(List<string> sMdPath, string sFilePath, string sPhpPath, string sNamesapce = null)
        {
            ErrorLog.ClearLogGroup();
            ErrorLog.ShowLogError("================={0}=================", true, DateTime.Now.ToString());

            if (sMdPath == null || sMdPath.Count <= 0)
            {
                Debug.LogError("md file does not exist");
                return;
            }
            else
            {
                ErrorLog.ShowLogError("md file count={0}", false, sMdPath.Count);
            }

            if (string.IsNullOrEmpty(sPhpPath))
            {
                Debug.LogError("orm.config.php file does not exist");
                return;
            }
            else
            {
                ErrorLog.ShowLogError("php file path={0}", false, sPhpPath);
            }

            if (string.IsNullOrEmpty(sFilePath))
            {
                Debug.LogError("generate path does not exist");
                return;
            }
            else
            {
                ErrorLog.ShowLogError("generate file path={0}", false, sFilePath);
            }

            var localTime = DateTime.Now;
            iTotal = localTime;

            //解析php文件
            Dictionary<string, string> tableNameDic;
            if (!ReaderPhp(sPhpPath, out tableNameDic)) return;

            TimeSpan tSpan = DateTime.Now.Subtract(localTime);
            ErrorLog.ShowLogError("readPhp=>count={1} time= {0} milliseconds", false, tSpan.Milliseconds, tableNameDic.Count);
            localTime = DateTime.Now;

            //解析md文件
            List<StructTable> structTableList;
            if (!ReaderMd(sMdPath, out structTableList)) return;

            tSpan = DateTime.Now.Subtract(localTime);
            ErrorLog.ShowLogError("readMd=>count={1} time= {0} milliseconds", false, tSpan.Milliseconds, structTableList.Count);
            localTime = DateTime.Now;

            //转换json数据及表名获取
            List<StructTable> tableGenerateList;
            if (!ChangeStructToJson(structTableList, tableNameDic, out tableGenerateList)) return;

            tSpan = DateTime.Now.Subtract(localTime);
            ErrorLog.ShowLogError("changeJsonAndTableName=>count={1} time= {0} milliseconds", false, tSpan.Milliseconds, tableGenerateList.Count);
            localTime = DateTime.Now;

            //生成class           
            myGenerateStruct.GenerateTypeStruct(sFilePath, tableGenerateList, sNamesapce);

            tSpan = DateTime.Now.Subtract(localTime);
            ErrorLog.ShowLogError("GenerateClass=>time= {0} milliseconds", false, tSpan.Milliseconds);

            tSpan = DateTime.Now.Subtract(iTotal);

            ErrorLog.ShowLogError(null, true);
            ErrorLog.ShowLogError(null, true);
            ErrorLog.ShowLogError("Finish Generate class count [{1}] ,total of time consuming [{0}] milliseconds", true, tSpan.Milliseconds, tableGenerateList.Count);
        }

        /// <summary>
        /// md文件读取
        /// </summary>
        /// <param name="pathList"></param>
        /// <param name="sturctTableList"></param>
        /// <returns></returns>
        bool ReaderMd(List<string> pathList, out List<StructTable> sturctTableList)
        {
            sturctTableList = new List<StructTable>();
            foreach (var item in pathList)
            {
                var structTable = myReadMD.ReadMdToStructTable(item);

                if (structTable.Count <= 0)
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(item);
                    ErrorLog.ShowLogError("{0}.md file is not found in table", true, fileName);
                    continue;
                }

                sturctTableList.AddRange(structTable);
            }

            if (sturctTableList.Count <= 0)
            {
                ErrorLog.ShowLogError("\nmd file does not exist", true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// php文件读取
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool ReaderPhp(string sPath, out Dictionary<string, string> tableName)
        {
            tableName = myReaderPhp.ReadPhpToStructTable(sPath);

            if (tableName == null || tableName.Count <= 0)
            {
                ErrorLog.ShowLogError("\nAll the tables do not exist", true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查找相应的表名及数据json系列化
        /// </summary>
        /// <param name="tableDataList"></param>
        /// <param name="tableNameDic"></param>
        /// <param name="tableGenerateList"></param>
        /// <returns></returns>
        bool ChangeStructToJson(List<StructTable> tableDataList, Dictionary<string, string> tableNameDic, out List<StructTable> tableGenerateList)
        {
            tableGenerateList = new List<StructTable>();
            foreach (var table in tableDataList)
            {
                if (tableNameDic.ContainsKey(table.tableName) || !table.isFindTableName)
                {
                    try
                    {
                        table.jsonTableData = JsonConvert.SerializeObject(table.tableField);
                    }
                    catch (Exception e)
                    {
                        ErrorLog.ShowLogError("[{0}] serialize json error", true, table.tableName);
                        continue;
                    }

                    if (tableNameDic.ContainsKey(table.tableName))
                        table.tableName = tableNameDic[table.tableName];
                }
                else
                {
                    ErrorLog.ShowLogError("[{0}] Table does not exist", true, table.tableName);
                    continue;
                }

                tableGenerateList.Add(table);
            }

            if (tableGenerateList.Count <= 0)
            {
                ErrorLog.ShowLogError("\nAll md file find tablename error", true);
                return false;
            }

            return true;
        }
    }

    public class ErrorLog
    {
        static StringBuilder errLog = new StringBuilder();
        static List<string> logList = new List<string>();

        /// <summary>
        /// 清空当前log
        /// </summary>
        public static void ClearLogGroup()
        {
            logList.Clear();
        }

        /// <summary>
        /// 清空所有log
        /// </summary>
        public static void Clear()
        {
            errLog.Remove(0, errLog.Length);
        }

        /// <summary>
        /// 添加log
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isError"></param>
        /// <param name="args"></param>
        public static void ShowLogError(string msg, bool isError, params object[] args)
        {
            if (!string.IsNullOrEmpty(msg))
                msg = string.Format(msg, args);

            if (isError) logList.Add(msg);
            //Debug.Log(msg);            
        }

        /// <summary>
        /// log信息
        /// </summary>
        public static string logInfo
        {
            get
            {
                if (errLog.Length > 0) errLog.Insert(0, "\n");

                var isError = (logList.Count >= 5);
                for (var i = logList.Count; i > 0; i--)
                {
                    var item = logList[i - 1];

                    errLog.Insert(0, "\n");

                    if (isError && i == 1)
                    {
                        errLog.Insert(0, "\nError\n");
                        isError = false;
                    }

                    if (!string.IsNullOrEmpty(item))
                    {
                        errLog.Insert(0, item);
                    }
                }

                ClearLogGroup();
                return errLog.ToString();
            }
        }
    }

}