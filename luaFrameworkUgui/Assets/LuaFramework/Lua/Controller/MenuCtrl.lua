
MenuCtrl = {};
local this = MenuCtrl;

local message;
local transform;
local gameObject;
local showCount;

--构建函数--
function MenuCtrl.New()
	logWarn("MenuCtrl.New--->>");
	return this;
end

function MenuCtrl.Awake()
	logWarn("MenuCtrl.Awake--->>");
	panelMgr:CreatePanel('Menu', this.OnCreate);
end

--启动事件--
function MenuCtrl.OnCreate(obj)
	showCount=0;
	
	gameObject = obj;	
	transform = obj.transform
	transform.localPosition  = Vector3.New(300,-220,0)

	message = gameObject:GetComponent('LuaBehaviour');
	message:AddClick(MenuPanel.btn_0, this.OnBtn0Click);

	logWarn("Start lua--->>"..gameObject.name);
end

--单击事件--
function MenuCtrl.OnBtn0Click(go)
	showCount = showCount + 1
	if showCount % 2 ==1 then
		SampleCtrl.Hide();
	else
	    SampleCtrl.Show();
	end
end

--关闭事件--
function MenuCtrl.Close()
	panelMgr:ClosePanel(CtrlNames.Message);
end