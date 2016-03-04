1.如果不需要解析数据类。（可删除ThirdParty文件夹及Networks/dataParser文件夹即可）

2.如何使用可以参考文档。（通信开发文档.docx）

	httpNetwork = HttpNetManager.Instance;
        httpNetwork.userID = "77"; //用户id
        httpNetwork.statusType = statusType;

        httpNetwork.RegisterResponse("game.login", ResponseHandler);  //单个接口的侦听
        httpNetwork.serverErrorResponse = ServerErrorHandler;
        httpNetwork.netTimeOut = NetTimeOutHandler;  //网络超时

        httpNetwork.RegisterNetworkDataParse(new NetworkDataParser()); //注入解析类，不注入会报错
        httpNetwork.RegisterTableDataStruct(new TestTableDataStruct());  //注入数据结构 new

        httpNetwork.Post("game.reset", ResponseHandler);
        httpNetwork.Post("game.login", ResponseHandler);  //单一侦听,报了系统级别错误不会有回调
        httpNetwork.Post("game.login", ResponseHandler);
        httpNetwork.Post("cityOrder.list", ResponseHandler);
        httpNetwork.Post("game.login", ResponseHandler);

        //测试单一的请求
        //  httpNetwork.PostOneToOne("game.login");
        //  httpNetwork.PostOneToOne("game.login", PostOneToOneHandler);
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}");
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}", PostOneToOneHandler);      

        TableDataManager.Instance.AddListenerDataTable("CityOrder", updateHandler); //注册侦听更改  new

3.例子请参考 http://dengshiwei@172.17.0.52:8080/git/untiy_framework_root.git 库里面的Network文件夹下面的unity例子
