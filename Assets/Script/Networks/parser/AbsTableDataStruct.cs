using System;
using System.Collections.Generic;

namespace Networks.parser
{
    /// <summary>
    /// 表结构数据
    /// </summary>
    public abstract class AbsTableDataStruct
    {
        protected Dictionary<string, Type> typeDict = new Dictionary<string, Type>(); //表数据

        /// <summary>
        /// 获取对应的类
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        /// <returns></returns>
        public Type findTableTypeData(string tableName)
        {
            Type type = null;
            if (typeDict.ContainsKey(tableName))
            {
                type = typeDict[tableName];
            }
            return type;
        }

        /// <summary>
        /// 注册绑定表格数据结构 （以下内容是sg项目组中的，其他项目组可以重新定义）
        /// </summary>
        public abstract void RegisterBindingTableStrcut();

        /// <summary>
        /// 消息通知，（可重载该方法，做自己项目的消息分发机制）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        public virtual void FireNotice(string tableName, object data)
        {

        }

    }
}
