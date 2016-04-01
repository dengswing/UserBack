using UnityEngine;
using Networks;
using Networks.parser;
using System.Collections.Generic;
using System.Text;

using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Networks.tool;
using Newtonsoft.Json;
using System.Reflection;
using Networks.log;

class Testing : MonoBehaviour
{
    public StatusType statusType = StatusType.NORMAL;

    HttpNetManager httpNetwork;
    void Start()
    {
        httpNetwork = HttpNetManager.Instance;
        httpNetwork.userID = "9"; //用户id
        httpNetwork.statusType = statusType;
        // httpNetwork.requestURL = "http://dev-soul.shinezoneapp.com/?&{0}";

        //access data (and print it)


        // http://dev-mi-facebook.shinezone.com/index.php?*=[["game.login",["1",0,0,1,"3","4"]]]&halt=711&sign=6YjG2QUodzBRd9RDQweiig2s3MQ%3D
        httpNetwork.requestURL = "http://dev-mi-facebook.shinezone.com/index.php?{0}";

        httpNetwork.RegisterResponse("game.login", ResponseHandler);  //单个接口的侦听
        httpNetwork.serverErrorResponse = ServerErrorHandler;
        httpNetwork.netTimeOut = NetTimeOutHandler;  //网络超时
        // httpNetwork.hamcKey = "key345"; //不传的话就默认不做api签名
        httpNetwork.hamcKey = "key001";

        httpNetwork.RegisterNetworkDataParse(new NetworkDataParser()); //注入解析类，不注入会报错

        httpNetwork.RegisterTableDataStruct(TestTableDataStruct.Instance);  //注入数据结构 new
        TestTableDataStruct.Instance.TableNotice += TableChange;

        var m = new Dictionary<string,string>();
        m.Add("s","1");

        // httpNetwork.Post("game.reset", ResponseHandler);
        httpNetwork.Post("game.init", ResponseHandler, 1, "2", "431",m);  //单一侦听,报了系统级别错误不会有回调
        // httpNetwork.Post("game.login", ResponseHandler);  //单一侦听,报了系统级别错误不会有回调
        // httpNetwork.Post("package.index", ResponseHandler);
        //httpNetwork.Post("cityOrder.list", ResponseHandler);
        // httpNetwork.Post("package.upgradeLv", ResponseHandler);

        //测试单一的请求
        //  httpNetwork.PostOneToOne("game.login");
        //  httpNetwork.PostOneToOne("game.login", PostOneToOneHandler);
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}");
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}", PostOneToOneHandler);      

        TableDataManager.Instance.AddListenerDataTable("ModuleProfile", UpdateHandler); //注册侦听更改  new
        TableDataManager.Instance.AddListenerDataTable("List", UpdateHandler); //注册侦听更改  new


        string text = "{ \"vb\":{\"x\":100,\"y\":200,\"magnitude\":10},\"userId\": 799,  \"grids\": {    \"1\": 10,    \"2\": 20,    \"3\": null,    \"4\": null,    \"5\": null,    \"6\": null,    \"7\": null,    \"8\": null  },  \"devices\": null,  \"decorations\": 567}";
        // string text = "{  \"userId\": [01,23]}";
        //string text = "{  \"userId\": 799}";

        CabinetInfo a = Newtonsoft.Json.JsonConvert.DeserializeObject(text, typeof(CabinetInfo)) as CabinetInfo;

        Debug.Log(a);

        var list = new ItemMakeCDInfoQueue();
        var listDict = new Dictionary<string, object>();
        listDict.Add("55", list);
        TableDataManager.Instance.AddTableData("List", listDict);
    }
    
    void TableChange(string tableName, object data)
    {
        Debug.Log("tableChange:" + tableName);
    }

    void UpdateHandler(object data)
    {
        Debug.Log(data);


        CabinetInfo cabinet = TableDataManager.Instance.GetTableData<CabinetInfo>("CabinetInfo");

        Dictionary<string, ModuleProfileInfo> dictModuleProfileInfo = TableDataManager.Instance.GetTableDataDictionary<ModuleProfileInfo>("ModuleProfileInfo");

        TableDataManager.Instance.RemoveTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000001);

        Dictionary<string, CityOrder> dictCabinet2 = TableDataManager.Instance.GetTableDataDictionary<CityOrder>("CityOrder");

        List<ItemMakeCDInfoQueue> list = TableDataManager.Instance.GetTableDataList<ItemMakeCDInfoQueue>("List");

        Dictionary<string, ItemMakeCDInfoQueue> dict = TableDataManager.Instance.GetTableDataDictionary<ItemMakeCDInfoQueue>("List");

        ModuleProfile info = TableDataManager.Instance.GetTableData<ModuleProfile>("ModuleProfile");

        List<CityOrder> cityInfo = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder");

        CityOrder cityInfo5 = TableDataManager.Instance.GetTableData<CityOrder>("CityOrder");
        CityOrder cityInfo6 = TableDataManager.Instance.GetTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000006);


        List<CityOrder> cityInfo2 = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000006);
        
        TableDataManager.Instance.RemoveTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000005);   //移除表格数据

        List<CityOrder> cityInfo3 = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder");

        TableDataManager.Instance.RemoveTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000001);   //移除表格数据
        TableDataManager.Instance.RemoveTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000002);   //移除表格数据
        TableDataManager.Instance.RemoveTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000007);   //移除表格数据
        //  TableDataManager.Instance.RemoveTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000008);   //移除表格数据

        List<CityOrder> cityInfo4 = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder");

        cityInfo = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000005);

        CityOrder cityOrder = TableDataManager.Instance.GetTableData<CityOrder>("CityOrder", x => x.cityOrderDefId == 50000005);
        Debug.Log(info);
        cityInfo = TableDataManager.Instance.GetTableData<List<CityOrder>>("CityOrder"); //错误读取
        Debug.Log(cityInfo);
    }



    void ResponseHandler(string cmd, int res, string value)
    {
        Debug.Log("Testing:ResponseHandler:response:" + cmd + "|" + res);

        Debug.Log(TableDataManager.Instance.GetTableData<string>("aa"));
        //Debug.Log(TableDataManager.Instance.currentResponseData);
    }

    void ServerErrorHandler(string cmd, int res, string value)
    {
        Debug.Log("Testing:ServerErrorHandler:response:" + cmd + "|" + res);
    }

    void NetTimeOutHandler()
    {
        Debug.Log("Testing:NetTimeOutHandler: request timeOut!");
    }

    void PostOneToOneHandler(string cmd, int res, string value)
    {
        Debug.Log("Testing:PostOneToOneHandler:response:" + cmd + "|" + res);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            httpNetwork.StartResetSend(); //超时了，重新在发一轮
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            httpNetwork.Clear(); //重置
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            httpNetwork.Clear(true); //重置
        }
    }

}