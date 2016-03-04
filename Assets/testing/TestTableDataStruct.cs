using Networks.parser;

/// <summary>
/// 表结构数据
/// </summary>
public class TestTableDataStruct : AbsTableDataStruct
{
    public delegate void TableChangeNotice(string tableName, object data);
    public TableChangeNotice TableNotice;

    public static readonly TestTableDataStruct Instance = new TestTableDataStruct();

    public override void RegisterBindingTableStrcut()
    {
        typeDict["ModuleProfile"] = typeof(ModuleProfile);
        typeDict["CityOrder"] = typeof(CityOrder);
        typeDict["List"] = typeof(ItemMakeCDInfoQueue);
        typeDict["CabinetInfo"] = typeof(CabinetInfo);

    }

    public override void FireNotice(string tableName, object data)
    {
        if (TableNotice != null) TableNotice(tableName, data);
    }

}

