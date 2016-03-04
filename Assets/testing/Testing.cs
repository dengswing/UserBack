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


class Testing : MonoBehaviour
{
    public StatusType statusType = StatusType.NORMAL;

    HttpNetManager httpNetwork;
    void Start()
    {
        httpNetwork = HttpNetManager.Instance;
        httpNetwork.userID = "799"; //用户id
        httpNetwork.statusType = statusType;

        httpNetwork.RegisterResponse("game.login", ResponseHandler);  //单个接口的侦听
        httpNetwork.serverErrorResponse = ServerErrorHandler;
        httpNetwork.netTimeOut = NetTimeOutHandler;  //网络超时
        httpNetwork.hamcKey = "key345"; //必须传

        httpNetwork.RegisterNetworkDataParse(new NetworkDataParser()); //注入解析类，不注入会报错

        httpNetwork.RegisterTableDataStruct(TestTableDataStruct.Instance);  //注入数据结构 new
        TestTableDataStruct.Instance.TableNotice += TableChange;


        // httpNetwork.Post("game.reset", ResponseHandler);
        httpNetwork.Post("game.login", ResponseHandler);  //单一侦听,报了系统级别错误不会有回调
        httpNetwork.Post("package.index", ResponseHandler);
        httpNetwork.Post("cityOrder.list", ResponseHandler);
        httpNetwork.Post("package.upgradeLv", ResponseHandler);

        //测试单一的请求
        //  httpNetwork.PostOneToOne("game.login");
        //  httpNetwork.PostOneToOne("game.login", PostOneToOneHandler);
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}");
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}", PostOneToOneHandler);      

        TableDataManager.Instance.AddListenerDataTable("ModuleProfile", UpdateHandler); //注册侦听更改  new

        string text = "{  \"userId\": 799,  \"grids\": {    \"1\": null,    \"2\": null,    \"3\": null,    \"4\": null,    \"5\": null,    \"6\": null,    \"7\": null,    \"8\": null  },  \"devices\": null,  \"decorations\": null}";

        CabinetInfo a = Newtonsoft.Json.JsonConvert.DeserializeObject(text, typeof(CabinetInfo)) as CabinetInfo;

        a.userId = 100;
        Debug.Log(a);

        string txt = "*=[[\"item.buy\",[1900,0,0,10030004,1,1]]]&halt=6";
        string txt1 = BaseBytes.EscapeUriString(txt);
        Debug.Log(txt1);
        string txt2 = BaseBytes.EscapeDataString(txt);
        Debug.Log(txt2);
        txt = "*=%5B%5B%22item.buy%22%2C%5B1900%2C0%2C0%2C10030004%2C1%2C1%5D%5D%5D&halt=6";

        string txt3 = BaseBytes.ToBase64StringData(BaseBytes.HashHmac(txt2, "key345", true), true);
        Debug.Log(txt3);

        string txt4 = BaseBytes.ToBase64StringData(BaseBytes.HashHmac(txt1, "key345", true), true);
        Debug.Log(txt4);

        ReadTxtToLst(Application.dataPath + "/Resources/test.md");
    }

    private void ReadTxtToLst(string spath) //listbox 读取txt文件
    {
        var _rstream = new StreamReader(spath, System.Text.Encoding.UTF8);
        string line;

        string way = _rstream.ReadToEnd();

        while ((line = _rstream.ReadLine()) != null)
        {
            Debug.Log(line);
        }
        _rstream.Close();
    }

    void TableChange(string tableName, object data)
    {
        Debug.Log("tableChange:" + tableName);
    }

    void UpdateHandler(object data)
    {
        Debug.Log(data);

        CabinetInfo cabinet = TableDataManager.Instance.GetTableData<CabinetInfo>("CabinetInfo");

        List<ItemMakeCDInfoQueue> list = TableDataManager.Instance.GetTableDataList<ItemMakeCDInfoQueue>("List");

        Dictionary<string, ItemMakeCDInfoQueue> dict = TableDataManager.Instance.GetTableDataDictionary<ItemMakeCDInfoQueue>("List");

        ModuleProfile info = TableDataManager.Instance.GetTableData<ModuleProfile>("ModuleProfile");
        List<CityOrder> cityInfo = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder");
        List<CityOrder> cityInfo2 = TableDataManager.Instance.GetTableDataList<CityOrder>("CityOrder");

        TableDataManager.Instance.RemoveTableList<CityOrder>("CityOrder");

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