local transform;
local gameObject;

MenuPanel = {};
local this = MenuPanel;

--�����¼�--
function MenuPanel.Awake(obj)
	gameObject = obj;
	transform = obj.transform;

	this.InitPanel();
	logWarn("Awake lua--->>"..gameObject.name);
end

--��ʼ�����--
function MenuPanel.InitPanel()
	this.btn_0 = transform:FindChild("node/btn_0").gameObject;
end

--�����¼�--
function MenuPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end

