using Networks.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Networks.parser
{
    /// <summary>
    /// 数据解析
    /// </summary>
    public class NetworkDataParser : INetworkDataParse
    {
        //服务器时间
        long _serverTime;

        //数据表管理
        TableDataManager dataTableManager = TableDataManager.Instance;

        /// <summary>
        /// 服务器时间
        /// </summary>
        public long serverTime
        {
            get { return _serverTime; }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="netstringBuff"></param>
        /// <param name="serverMsg"></param>
        /// <returns></returns>
        public int ParseData(string netstringBuff, out string serverMsg)
        {
           // netstringBuff = "{\"code\": 0,\"msg\": [{\"result\": 1}],\"gmt\": 1448872840,\"UPDATE\": {  \"ModuleProfileInfo\": {    \"156\": {      \"5\": {\"info\": 2}    }  },  \"ModuleProfile\": {\"156\": {\"token\": 47500}},  \"List\": {    \"156\": {      \"5\": {        \"listInfo\": [          {            \"itemDefId\": 10000003,            \"cd\": 35,            \"completeTime\": 1448872816,            \"material\": [              {\"10010011\": 1},              {\"10010012\": 1}            ]          },          {            \"itemDefId\": 10000011,            \"cd\": 35,            \"completeTime\": 1448872851,            \"material\": [              {\"10010008\": 3},              {\"10010020\": 1},              {\"10010023\": 1}            ]          }        ]      }    }  },  \"RobotInfo\": {    \"156\": {      \"700002\": {        \"proficiency\": 2,        \"isUpgrade\": 0,        \"progress\": [          {            \"entityId\": 50110030005,            \"count\": 0          },          {            \"entityId\": 50110030006,            \"count\": 0          },          {            \"entityId\": 50110030007,            \"count\": 0          }        ]      }    }  },  \"OldElectricNpc\": {    \"156\": {      \"607\": {        \"electricNpcDefId\": 0,        \"carryOldElectricDefId\": 0,        \"lastLeaveTime\": 1448872840      }    }  }}}";
            IServerResponseData objServerResponseData = JsonDataManager.Instance.ParseJsonDataFromServer(netstringBuff);
            dataTableManager.currentResponseData = objServerResponseData;

            serverMsg = objServerResponseData.errMsg;
            _serverTime = objServerResponseData.serverTime;

            if (objServerResponseData.code == HttpNetManager.RESPONSE_CODE_RESULT_SUCCESS) AllDataChangeStruct(objServerResponseData);
            return objServerResponseData.code;
        }

        /// <summary>
        /// 所有数据转换成结构
        /// </summary>
        /// <param name="data"></param>
        void AllDataChangeStruct(IServerResponseData data)
        {
            //基础内容
            data.msgListTableStruct = new List<Dictionary<string, object>>();
            Dictionary<string, object> allTableData;
            foreach (var i in data.msgListData)
            {
                allTableData = TableChangeStruct(i, true);
                data.msgListTableStruct.Add(allTableData);
            }

            if (data.updateListData == null) return;
            data.updataListTableStruct = TableChangeStruct(data.updateListData); //更新的内容

            foreach (var j in data.updataListTableStruct)
            {
                dataTableManager.FireNotice(j.Key, j.Value);
            }
        }

        /// <summary>
        /// 根据数据库表格来转换结构
        /// </summary>
        /// <param name="jsonData">所有表格数据</param>
        /// <returns></returns>
        Dictionary<string, object> TableChangeStruct(Dictionary<string, object> jsonData, bool isAdd = false)
        {
            Dictionary<string, object> allTableData = new Dictionary<string, object>(); //表结构 string表名、object表数据[按记录存]   
            foreach (var i in jsonData)
            {
                object tableStruct = TableStructConstructor(i.Key, i.Value, !isAdd);
                allTableData.Add(i.Key, tableStruct);
                if (isAdd) dataTableManager.AddTableData(i.Key, tableStruct);
            }

            return allTableData;
        }

        /// <summary>
        /// 映射数据结构
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="value">数据</param>
        /// <returns></returns>
        object TableStructConstructor(string tableName, object value, bool isUpdate = false)
        {
            Type table = dataTableManager.findTableTypeData(tableName);
            if (table == null) return StringChangeValue(value.ToString());  //数据结构不存在，直接返回值
            object tableData = MatchingTableStruct(tableName, value, table, isUpdate);
            return tableData;
        }

        /// <summary>
        /// 表结构匹配，支持多条
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="value">数据</param>
        /// <param name="table">类结构体</param>
        /// <returns></returns>
        object MatchingTableStruct(string tableName, object value, Type classStruct, bool isUpdate = false)
        {
            Dictionary<string, object> tableStruct = JsonDataManager.Instance.GetTableStruct(value); //转换json数据

            object tableObj = dataTableManager.GetTableDataObject(tableName);
            Dictionary<string, object> tableListData;

            if (isUpdate && tableObj != null)
            {
                tableListData = tableObj as Dictionary<string, object>;
            }
            else
            {
                tableListData = new Dictionary<string, object>();
            }

            string key = null;
            string dataValue;
            bool isUpdateData = isUpdate;
            object tableData;
            foreach (var list in tableStruct)
            { //每条记录赋值
                isUpdateData = isUpdate;
                dataValue = list.Value as string;
                key = list.Key;

                if (isUpdate && !tableListData.ContainsKey(key))
                {//表示是新加.list增加
                    isUpdateData = false;
                }

                if (isUpdateData)
                {
                    tableData = tableListData[key];
                    UpdateFieldValue(dataValue, classStruct, tableData);
                }
                else
                {
                    try
                    {
                        tableData = Newtonsoft.Json.JsonConvert.DeserializeObject(dataValue, classStruct);
                    }
                    catch (Exception e)
                    {
                        tableData = null;
                    }
                }

                if (!isUpdateData)
                {
                    dataTableManager.AddTableListData(tableName, tableData);    //增加列表
                    tableListData.Add(key, tableData);
                }
            }
			
			if (isUpdate && tableObj == null) dataTableManager.AddTableData(tableName, tableListData);		
			
            return tableListData;
        }

        void UpdateFieldValue(string dataValue, Type classStruct, object tableData)
        {
            Newtonsoft.Json.Linq.JObject jsonValue = Newtonsoft.Json.Linq.JObject.Parse(dataValue);
            object updateTableData = Newtonsoft.Json.JsonConvert.DeserializeObject(dataValue, classStruct);

            foreach (var data in jsonValue)
            { //每个字段赋值
                Type inst_type = tableData.GetType();
                FieldInfo fieldInfo = inst_type.GetField(data.Key);
                PropertyInfo propertyInfo = inst_type.GetProperty(data.Key);
                Type update_type = updateTableData.GetType();
                FieldInfo upFieldInfo = update_type.GetField(data.Key);
                PropertyInfo upPropertyInfo = update_type.GetProperty(data.Key);

                object newData = null;
                if (upFieldInfo != null)
                    newData = upFieldInfo.GetValue(updateTableData);
                else if (upPropertyInfo != null)
                    newData = upPropertyInfo.GetValue(updateTableData, null);

                if (fieldInfo != null)
                    fieldInfo.SetValue(tableData, newData);
                else if (propertyInfo != null)
                    propertyInfo.SetValue(tableData, newData, null);
            }
        }

        /// <summary>
        /// 更新字段的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fieldName"></param>
        /// <param name="stringValue"></param>
        /// <param name="key"></param>
        void UpdateTableFieldValue(Dictionary<string, object> data, string fieldName, string stringValue, string key)
        {
            if (data.ContainsKey(key))
                SetFieldValue(data[key], fieldName, stringValue);
        }

        /// <summary>
        /// 设置表中的对象值
        /// </summary>
        /// <param name="classInstance">类</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="stringValue">内容</param>
        void SetFieldValue(object classInstance, string fieldName, string stringValue)
        {
            Type inst_type = classInstance.GetType();
            FieldInfo fieldInfo = inst_type.GetField(fieldName);

            Type underlying_type = Nullable.GetUnderlyingType(inst_type);
            Type value_type = underlying_type ?? inst_type;

            Type fieldType = null;
            if (null != fieldInfo)
            {
                fieldType = fieldInfo.FieldType;
                object obj = ConvertFromStr(stringValue.Trim(), fieldType);
                try
                {
                    fieldInfo.SetValue(classInstance, obj);
                }
                catch (Exception ex)
                { //类结构的属性和表字段不匹配了
                    UnityEngine.Debug.LogWarning(value_type.FullName + ":" + fieldName + "=" + stringValue + " Error:" + ex.Message);
                }

                System.Reflection.MethodInfo mInfo = inst_type.GetMethod("ParseField");
                if (mInfo != null)
                {//触发ParseField方法来处理特殊字段。因为目前类和表只支持键值
                    mInfo.Invoke(classInstance, new object[] { fieldName, stringValue });
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("fieldName:" + fieldName + " " + classInstance.GetType().ToString());
            }
        }

        /// <summary>
        /// 根据类型转换相应的数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        object ConvertFromStr(string value, Type valueType)
        {
            Type underlying_type = Nullable.GetUnderlyingType(valueType);
            Type value_type = underlying_type ?? valueType;
            if (value_type == typeof(string))
            {
                return value;
            }
            else if (value_type == typeof(bool))
            {
                bool n_bool = false;
                bool.TryParse(value, out n_bool);
                return n_bool;
            }
            else
            {
                return StringChangeValue(value);
            }
        }

        /// <summary>
        /// 把字符串转换成实际的类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        object StringChangeValue(string value, Type valueType = null)
        {
            if (value.IndexOf("{") >= 0 || value.IndexOf("[") >= 0)
            {
                return value;
                //object retObj = null;
                //try
                //{
                //    if (null == valueType)
                //        retObj = JsonConvert.DeserializeObject(value);
                //    else
                //        retObj = JsonConvert.DeserializeObject(value, valueType);
                //}
                //catch (Exception e)
                //{
                //    UnityEngine.Debug.LogWarning(e.Message);
                //}
                //return retObj;
            }

            if (value.IndexOf('.') != -1 ||
                value.IndexOf('e') != -1 ||
                value.IndexOf('E') != -1)
            {

                double n_double;
                if (Double.TryParse(value, out n_double))
                {
                    return n_double;
                }
            }

            int n_int32;
            if (Int32.TryParse(value, out n_int32))
            {
                return n_int32;
            }

            long n_int64;
            if (Int64.TryParse(value, out n_int64))
            {
                return n_int64;
            }

            ulong n_uint64;
            if (UInt64.TryParse(value, out n_uint64))
            {
                return n_uint64;
            }

            bool n_bool;
            if (Boolean.TryParse(value, out n_bool))
            {
                return n_bool;
            }

            return value;
        }

    }
}