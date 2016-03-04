using Networks.interfaces;
using System;
using System.Collections.Generic;

namespace Networks.parser
{
    /// <summary>
    /// 表结构数据管理
    /// </summary>
    public class TableDataManager : IDataTablePull
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public static readonly TableDataManager Instance = new TableDataManager();

        //当前请求返回的结果
        IServerResponseData _currentResponseData;
        Dictionary<string, DataTableUpdateDelegate> dataTableListener = new Dictionary<string, DataTableUpdateDelegate>(); //数据表变更委托
        Dictionary<string, object> dataTableAll = new Dictionary<string, object>(); //所有的数据表数据
        AbsTableDataStruct _tableDataStruct;  //表结构数据
        Dictionary<string, object> dataTableList = new Dictionary<string, object>();  //表格数据数组缓存,（只存被获取过一次的缓存数据）

        /// <summary>
        /// 设置表结构
        /// </summary>
        public AbsTableDataStruct tableDataStruct
        {
            set
            {
                _tableDataStruct = value;
                _tableDataStruct.RegisterBindingTableStrcut();
            }
        }

        /// <summary>
        /// 当前请求返回的结果
        /// </summary>
        public IServerResponseData currentResponseData
        {
            get { return _currentResponseData; }
            set { _currentResponseData = value; }
        }

        /// <summary>
        /// 获取对应的类
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        /// <returns></returns>
        public Type findTableTypeData(string tableName)
        {
            if (_tableDataStruct == null) return null;
            return _tableDataStruct.findTableTypeData(tableName);
        }

        /// <summary>
        /// 增加表格数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        public void AddTableData(string tableName, object data)
        {
            dataTableAll[tableName] = data;
            if (data == null) dataTableAll.Remove(tableName);
        }

        /// <summary>
        /// 增加表格list数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        public void AddTableListData(string tableName, object data)
        {
            if (dataTableList.ContainsKey(tableName))
            {
                List<object> dataList = GetTableDataList<object>(tableName);
                dataList.Add(data);
            }
        }

        /// <summary>
        /// 返回数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public T GetTableData<T>(string tableName)
        {
            try
            {
                return GetTableDataDispose<T>(tableName);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 返回数据表，（列表中筛选一个）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public T GetTableData<T>(string tableName, Predicate<T> cond)
        {
            try
            {
                return GetTableDataDispose<T>(tableName, cond);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 返回数据表（列表形式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public List<T> GetTableDataList<T>(string tableName, Predicate<T> cond = null)
        {
            try
            {
                return GetTableDataListDispose<T>(tableName, cond);
            }
            catch (Exception e)
            {
                return default(List<T>);
            }
        }

        /// <summary>
        /// 返回数据表（字典形式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public Dictionary<string, T> GetTableDataDictionary<T>(string tableName, Predicate<T> cond = null)
        {
            try
            {
                return GetTableDataDictionaryDispose<T>(tableName, cond);
            }
            catch (Exception e)
            {
                return default(Dictionary<string, T>);
            }
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public object GetTableDataObject(string tableName)
        {
            if (dataTableAll.ContainsKey(tableName))
                return dataTableAll[tableName];
            else
                return null;
        }

        /// <summary>
        /// 移除缓存的表格list
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool RemoveTableList<T>(string tableName, Predicate<T> cond = null)
        {
            if (!dataTableList.ContainsKey(tableName)) return false;

            List<T> tableData = (List<T>)dataTableList[tableName];

            if (cond == null)
            {
                tableData.Clear();
            }
            else
            {
                for (int i = tableData.Count; i > 0; i--)
                {
                    if (cond.Invoke((T)tableData[i - 1]))
                        tableData.RemoveAt(i - 1);
                }
            }

            if (tableData.Count <= 0) dataTableList.Remove(tableName);

            return true;
        }

        /// <summary>
        /// 移除表格数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        public bool RemoveTableData<T>(string tableName, Predicate<T> cond = null)
        {
            if (!dataTableAll.ContainsKey(tableName))
                return false;

            Dictionary<string, object> tableData = (Dictionary<string, object>)dataTableAll[tableName];

            RemoveTableList<T>(tableName, cond);

            if (cond == null)
            {
                tableData.Clear();
            }
            else
            {
                List<string> listObj = new List<string>();
                foreach (var kv in tableData)
                {
                    if (cond.Invoke((T)kv.Value))
                        listObj.Add(kv.Key);
                }

                while (listObj.Count > 0)
                {
                    tableData.Remove(listObj[0]);
                    listObj.RemoveAt(0);
                }
            }

            if (tableData.Count <= 0) dataTableAll.Remove(tableName);

            return true;
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        public void FireNotice(string tableName, object data)
        {
            if (dataTableListener.ContainsKey(tableName))
                dataTableListener[tableName](data);

            _tableDataStruct.FireNotice(tableName, data);
        }

        /// <summary>
        /// 增加数据表委托
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateBack"></param>
        public void AddListenerDataTable(string tableName, DataTableUpdateDelegate updateBack)
        {
            if (dataTableListener.ContainsKey(tableName))
            {
                dataTableListener[tableName] -= updateBack;
                dataTableListener[tableName] += updateBack;
            }
            else
            {
                dataTableListener[tableName] = updateBack;
            }
        }

        /// <summary>
        /// 移除数据表委托
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateBack"></param>
        public void RemoveListenerDataTable(string tableName, DataTableUpdateDelegate updateBack)
        {
            if (dataTableListener.ContainsKey(tableName))
                dataTableListener[tableName] -= updateBack;
        }

        private T GetTableDataDispose<T>(string tableName, Predicate<T> cond = null)
        {
            T data = default(T);
            if (dataTableAll.ContainsKey(tableName))
            {
                object obj;
                if (dataTableAll.TryGetValue(tableName, out obj))
                {
                    if (obj is Dictionary<string, object>)
                    {
                        Dictionary<string, object> tableData = (Dictionary<string, object>)obj;
                        foreach (var j in tableData)
                        {
                            if (cond == null || cond.Invoke((T)j.Value))
                            {
                                data = (T)j.Value;
                                break;
                            }
                        }
                        tableData = null;
                    }
                }
                obj = null;
            }
            return data;
        }

        private List<T> GetTableDataListDispose<T>(string tableName, Predicate<T> cond = null)
        {
            List<T> data = default(List<T>);
            if (dataTableAll.ContainsKey(tableName))
            {
                if (cond == null && dataTableList.ContainsKey(tableName))
                {
                    data = (List<T>)dataTableList[tableName]; //直接取已经缓存的 
                    return data;
                }

                object obj;
                if (dataTableAll.TryGetValue(tableName, out obj))
                {
                    if (obj is Dictionary<string, object>)
                    {
                        List<object> listObj = new List<object>();

                        Dictionary<string, object> tableData = (Dictionary<string, object>)obj;
                        foreach (var j in tableData)
                        {
                            if (cond != null && !cond.Invoke((T)j.Value))
                            {
                                continue;
                            }
                            listObj.Add(j.Value);
                        }

                        if (listObj.Count > 0)
                        {
                            data = new List<T>();
                            listObj.ForEach(x => data.Add((T)x));
                            if (!dataTableList.ContainsKey(tableName)) dataTableList.Add(tableName, data);
                        }

                        listObj = null;
                        tableData = null;
                    }
                }
                obj = null;
            }
            return data;
        }

        private Dictionary<string, T> GetTableDataDictionaryDispose<T>(string tableName, Predicate<T> cond = null)
        {
            Dictionary<string, T> data = default(Dictionary<string, T>);
            if (dataTableAll.ContainsKey(tableName))
            {
                object obj;
                if (dataTableAll.TryGetValue(tableName, out obj))
                {
                    if (obj is Dictionary<string, object>)
                    {
                        data = new Dictionary<string, T>();
                        Dictionary<string, object> tableData = (Dictionary<string, object>)obj;
                        foreach (var j in tableData)
                        {
                            if (cond != null && !cond.Invoke((T)j.Value))
                            {
                                continue;
                            }
                            data.Add(j.Key, (T)j.Value);
                        }

                        tableData = null;
                    }
                }
                obj = null;
            }
            return data;
        }
    }
}