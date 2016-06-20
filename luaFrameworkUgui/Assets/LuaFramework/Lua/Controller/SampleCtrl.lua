require "Common/define"

SampleCtrl={}
local this = SampleCtrl

local transform
local gameObject
local lua


function SampleCtrl.New()
	logWarn("SampleCtrl.New--->>");
	return this;
end

function SampleCtrl.Awake()
	logWarn("SampleCtrl.Awake--->>")
	panelMgr:CreatePanel('Sample', this.OnCreate);
end

function SampleCtrl.OnCreate(obj)
	print("SampleCtrl.OnCreate");
	gameObject = obj;
	transform = obj.transform

	lua = gameObject:GetComponent("LuaBehaviour")
	lua:AddClick( SamplePanel.btnClose, this.OnBtnClick)
end

function SampleCtrl.OnBtnClick()
	print("SampleCtrl.OnBtnClose");
	--gameObject:SetActive(false)
	SamplePanel.bg:SetActive(false);
end

function SampleCtrl.Show()
		--ialogPanel.anim:SetBool("IsShow",true)
		gameObject:SetActive(true);
end
function SampleCtrl.Hide()
		--DialogPanel.anim:SetBool("IsShow",false)
		gameObject:SetActive(false);
end

function SampleCtrl.Destroy()		
		destroy(gameObject);
end