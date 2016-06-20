local transform;
local gameObject;

MenuPanel = {};
local this = MenuPanel;

--启动事件--
function MenuPanel.Awake(obj)
	gameObject = obj;
	transform = obj.transform;

	this.InitPanel();
	logWarn("Awake lua--->>"..gameObject.name);
end

--初始化面板--
function MenuPanel.InitPanel()
	this.btn_0 = transform:FindChild("node/btn_0").gameObject;
end

--单击事件--
function MenuPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end

