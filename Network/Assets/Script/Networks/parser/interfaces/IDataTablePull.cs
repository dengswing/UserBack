namespace Networks.interfaces
{
    public interface IDataTablePull
    {
        /// <summary>
        /// 表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        System.Type findTableTypeData(string tableName);

        /// <summary>
        /// 当前请求数据
        /// </summary>
        IServerResponseData currentResponseData { get; set; }

        /// <summary>
        /// 数据变更通知
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">数据</param>
        void FireNotice(string tableName, object data);       

        /// <summary>
        /// 添加表数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        void AddTableData(string tableName, object data);

        /// <summary>
        /// 增加表格list数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        void AddTableListData(string tableName, object data);

        //===================================================================================
        //===================================================================================

        /// <summary>
        /// 返回表数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        T GetTableData<T>(string tableName);

        /// <summary>
        /// 返回数据表，（列表中筛选一个）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        T GetTableData<T>(string tableName, System.Predicate<T> cond);

        /// <summary>
        /// 返回数据表（列表形式）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        System.Collections.Generic.List<T> GetTableDataList<T>(string tableName, System.Predicate<T> cond = null);

        /// <summary>
        /// 返回数据表（字典形式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        System.Collections.Generic.Dictionary<string, T> GetTableDataDictionary<T>(string tableName, System.Predicate<T> cond = null);

        /// <summary>
        /// 侦听表更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateBack"></param>
        void AddListenerDataTable(string tableName, DataTableUpdateDelegate updateBack);

        /// <summary>
        /// 移除表更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateBack"></param>
        void RemoveListenerDataTable(string tableName, DataTableUpdateDelegate updateBack);

        /// <summary>
        /// 移除缓存的表格list(缓存为List的数据)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool RemoveTableList<T>(string tableName, System.Predicate<T> cond = null);

        /// <summary>
        /// 移除表格数据 (缓存为Dictionary的数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        bool RemoveTableData<T>(string tableName, System.Predicate<T> cond = null);

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        void RemoveAllData();
    }
}