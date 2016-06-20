
SamplePanel = {};
local this = SamplePanel

local transform
local gameObject

function SamplePanel.Awake(obj)
		gameObject = obj;
		transform = gameObject.transform
		
		this.InitPanel()
end

function SamplePanel.InitPanel()
	this.btnClose = transform:FindChild("btn_0").gameObject
	this.bg = transform:FindChild("bg").gameObject--:GetComponent('UnityEngine.UI.Image');
end