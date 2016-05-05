using Networks.data;
using Networks.interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
namespace Networks.parser
{
    class JsonDataManager
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public static readonly JsonDataManager Instance = new JsonDataManager();

        const string SERVER_RESPONSE_CODE = "code"; //处理结果
        const string SERVER_RESPONSE_MSG = "msg";   //内容
        const string SERVER_RESPONSE_GMT = "gmt";   //服务器时间
        const string SERVER_RESPONSE_UPDATE = "UPDATE"; //更新数据
        const string SERVER_RESPONSE_DELETE = "DELETE"; //删除

        //错误key
        const int RESPONSE_CODE_ERROR = -999;

        /// <summary>
        /// 解析服务器通过 http response 传来的所有数据内容，也就是在浏览器中能看到的全部信息
        /// </summary>
        public IServerResponseData ParseJsonDataFromServer(string jsonDataFromServer)
        {
            JObject jObjectRoot = JObject.Parse(jsonDataFromServer);
            IServerResponseData serverData = ParseJsonToStructData(jObjectRoot);
            return serverData;
        }

        /// <summary>
        /// 表结构数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetTableStruct(object data)
        {
            Dictionary<string, object> tableStructList = new Dictionary<string, object>();
            // JsonStructToKeyValue((JObject)data, tableStructList);

            JsonStructToKeyValueNew((JObject)data, tableStructList);
            return tableStructList;
        }

        public List<string> GetTableDeleteList(object data)
        {
            List<string> deleteList = new List<string>();
            GetArrayValue(data, deleteList);
            return deleteList;
        }

        void GetArrayValue(object data, List<string> list)
        {
            if (data is JArray)
            {
                foreach (var value in (JArray)data)
                {
                    list.Add(value.ToString());
                }
            }
            else if (data is JObject)
            {
                foreach (JProperty property in ((JObject)data).Children())
                {
                    GetArrayValue(property.Value, list);
                }
            }
            else
            {
                list.Add(data.ToString());
            }
        }

        /// <summary>
        /// json数据包装成数据结构
        /// </summary>
        /// <param name="rootData">json数据</param>
        /// <returns></returns>
        IServerResponseData ParseJsonToStructData(JObject rootData)
        {
            IServerResponseData objServerResponseData = new ServerResponseData();

            JProperty jPropertyCode = null;
            JProperty jPropertyMsg = null;
            JProperty jPropertyUpdate = null;
            JProperty jPropertyGmt = null;
            JProperty jPropertyDelete = null;

            foreach (JProperty jPropertyRootItem in rootData.Children())
            {
                switch (jPropertyRootItem.Name)
                {
                    case SERVER_RESPONSE_CODE:
                        jPropertyCode = jPropertyRootItem;
                        break;
                    case SERVER_RESPONSE_MSG:
                        jPropertyMsg = jPropertyRootItem;
                        break;
                    case SERVER_RESPONSE_UPDATE:
                        jPropertyUpdate = jPropertyRootItem;
                        break;
                    case SERVER_RESPONSE_GMT:
                        jPropertyGmt = jPropertyRootItem;
                        break;
                    case SERVER_RESPONSE_DELETE:
                        jPropertyDelete = jPropertyRootItem;
                        break;
                    default:
                        break;
                }
            }

            if (jPropertyCode != null)
            {
                objServerResponseData.code = ParseCode(jPropertyCode);
                objServerResponseData.serverTime = long.Parse(jPropertyGmt.Value.ToString());
                if (HttpNetManager.RESPONSE_CODE_RESULT_SUCCESS == objServerResponseData.code)
                {
                    if (jPropertyMsg != null)
                    {
                        var msgValue = jPropertyMsg.Value;
                        if (msgValue is JArray)
                        {
                            objServerResponseData.result = Newtonsoft.Json.JsonConvert.SerializeObject(msgValue.First);
                        }
                        else if (msgValue is JObject)
                        {
                            JProperty firstChild = (JProperty)msgValue.First;
                            if (firstChild != null)
                                objServerResponseData.result = Newtonsoft.Json.JsonConvert.SerializeObject(firstChild.Value);
                        }

                        objServerResponseData.msgListData = ParseMsg(jPropertyMsg);
                    }

                    if (jPropertyUpdate != null)
                    {
                        objServerResponseData.updateListData = ParseUpdate(jPropertyUpdate);
                    }

                    if (jPropertyDelete != null)
                    {
                        objServerResponseData.deleteListData = ParseUpdate(jPropertyDelete);
                    }

                }
                else
                {
                    if (jPropertyMsg != null)
                    {
                        objServerResponseData.errMsg = jPropertyMsg.Value.ToString();
                    }
                }
            }

            return objServerResponseData;
        }

        /// <summary>
        /// json数据包装成键值对应
        /// </summary>
        /// <param name="tabel">表给数据</param>
        /// <param name="tableStructList">表格结构列表</param>
        void JsonStructToKeyValueNew(JObject tabel, Dictionary<string, object> tableStructList)
        {
            foreach (JProperty jMsg in tabel.Children())
            {
                string tableList = null;

                var sValue = jMsg.Value.ToString();
                if (sValue == "[]" || sValue == "{}" || string.IsNullOrEmpty(sValue))
                {
                    continue;
                }

                foreach (JObject jNode in jMsg.Children())
                {
                    JProperty firstChild = (JProperty)jNode.First;
                    if (firstChild == null)
                    {
                        tableList = string.Empty;
                    }
                    else if (IsSimpleValue(firstChild) || !IsNumberKey(firstChild))
                    {
                        tableList = jMsg.Value.ToString();
                    }
                    else
                    {
                        JsonStructToKeyValueNew(jNode, tableStructList);
                    }
                }

                if (!string.IsNullOrEmpty(tableList)) tableStructList.Add(jMsg.Name, tableList);
            }
        }


        /// <summary>
        /// json数据包装成键值对应
        /// </summary>
        /// <param name="tabel">表给数据</param>
        /// <param name="tableStructList">表格结构列表</param>
        void JsonStructToKeyValue(JObject tabel, Dictionary<string, object> tableStructList)
        {
            foreach (JProperty jMsg in tabel.Children())
            {
                Dictionary<string, string> tableList = new Dictionary<string, string>();
                foreach (JObject jNode in jMsg.Children())
                {
                    JProperty firstChild = (JProperty)jNode.First;
                    if (IsSimpleValue(firstChild) || !IsNumberKey(firstChild))
                    { //2层结构
                        foreach (JProperty property in jNode.Children())
                        {
                            tableList.Add(property.Name, property.Value.ToString());
                        }
                    }
                    else
                    { //3层结构
                        JsonStructToKeyValue(jNode, tableStructList);
                        goto Found; //防止多生成一条数据
                    }
                }
                tableStructList.Add(jMsg.Name, tableList);
            }

            Found:
            return;
        }

        /// <summary>
        /// 解析code值
        /// </summary>
        /// <param name="jProperty"></param>
        /// <returns></returns>
        int ParseCode(JProperty jProperty)
        {
            int code = RESPONSE_CODE_ERROR;
            if (IsSimpleValue(jProperty))
            {
                code = Int32.Parse(jProperty.Value.ToString());
            }
            else
            {
                throw new Exception();
            }

            return code;
        }

        /// <summary>
        /// 解析msg值
        /// </summary>
        /// <param name="jPropertyMsgRoot"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> ParseMsg(JProperty jPropertyMsgRoot)
        {
            List<Dictionary<string, object>> msgListData = new List<Dictionary<string, object>>();


            foreach (JContainer jMsgItem in jPropertyMsgRoot.Children())
            {
                if (jMsgItem is JArray)
                {
                    foreach (JObject jMsgObjectRoot in jMsgItem)
                    {
                        msgListData.Add(ParseJsonStruct(jMsgObjectRoot));
                    }
                }
                else if (jMsgItem is JObject)
                {
                    foreach (JProperty property in jMsgItem.Children())
                    {
                        if (IsSimpleValue(property))
                        {
                            continue;
                        }

                        foreach (JObject jMsgObjectRoot in property.Children())
                        {
                            msgListData.Add(ParseJsonStruct(jMsgObjectRoot));
                        }
                    }
                }

            }

            return msgListData;
        }

        /// <summary>
        /// 解析update值
        /// </summary>
        /// <param name="jPropertyUpdateRoot"></param>
        /// <returns></returns>
        Dictionary<string, object> ParseUpdate(JProperty jPropertyUpdateRoot)
        {
            Dictionary<string, object> listData = null;
            foreach (JObject jPropertyObject in jPropertyUpdateRoot.Children())
            {
                listData = ParseJsonStruct(jPropertyObject);
            }

            return listData;
        }

        /// <summary>
        /// 解析json数据第一层，表名-->内容
        /// </summary>
        /// <param name="jPropertyObject"></param>
        /// <returns></returns>
        Dictionary<string, object> ParseJsonStruct(JObject jPropertyObject)
        {
            Dictionary<string, object> listData = new Dictionary<string, object>();

            foreach (JProperty jUpdateProperty in jPropertyObject.Children())
            {
                listData.Add(jUpdateProperty.Name, jUpdateProperty.Value);
            }

            return listData;
        }

        /// <summary>
        /// 判断是否简单值类型，即它的值应该不是对象，不是数组，而是 string 或 int，【如 "A" : abc】
        /// </summary>
        bool IsSimpleValue(JProperty jProperty)
        {
            bool isSimpleValue = false;

            if (jProperty.Count == 1)
            {
                JToken firstChild = jProperty.First;
                if (firstChild is JValue)
                {
                    isSimpleValue = true;
                }
            }

            return isSimpleValue;
        }

        /// <summary>
        /// 是否数字为key
        /// </summary>
        /// <param name="jprop"></param>
        /// <returns></returns>
        bool IsNumberKey(JProperty jprop)
        {
            if (jprop == null)
                return false;
            string str = jprop.Name;
            int i = 0; long k = 0;
            if (!int.TryParse(str, out i))
            {
                return long.TryParse(str, out k);
            }
            return true;
        }

    }
}
