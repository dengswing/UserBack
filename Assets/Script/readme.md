1.�������Ҫ���������ࡣ����ɾ��ThirdParty�ļ��м�Networks/dataParser�ļ��м��ɣ�

2.���ʹ�ÿ��Բο��ĵ�����ͨ�ſ����ĵ�.docx��

	httpNetwork = HttpNetManager.Instance;
        httpNetwork.userID = "77"; //�û�id
        httpNetwork.statusType = statusType;

        httpNetwork.RegisterResponse("game.login", ResponseHandler);  //�����ӿڵ�����
        httpNetwork.serverErrorResponse = ServerErrorHandler;
        httpNetwork.netTimeOut = NetTimeOutHandler;  //���糬ʱ

        httpNetwork.RegisterNetworkDataParse(new NetworkDataParser()); //ע������࣬��ע��ᱨ��
        httpNetwork.RegisterTableDataStruct(new TestTableDataStruct());  //ע�����ݽṹ new

        httpNetwork.Post("game.reset", ResponseHandler);
        httpNetwork.Post("game.login", ResponseHandler);  //��һ����,����ϵͳ������󲻻��лص�
        httpNetwork.Post("game.login", ResponseHandler);
        httpNetwork.Post("cityOrder.list", ResponseHandler);
        httpNetwork.Post("game.login", ResponseHandler);

        //���Ե�һ������
        //  httpNetwork.PostOneToOne("game.login");
        //  httpNetwork.PostOneToOne("game.login", PostOneToOneHandler);
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}");
        // httpNetwork.PostOneToOne("game.login", "Http://test.com/&*={0}", PostOneToOneHandler);      

        TableDataManager.Instance.AddListenerDataTable("CityOrder", updateHandler); //ע����������  new

3.������ο� http://dengshiwei@172.17.0.52:8080/git/untiy_framework_root.git �������Network�ļ��������unity����
