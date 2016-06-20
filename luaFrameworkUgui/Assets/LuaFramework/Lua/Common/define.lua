
CtrlNames = {
	--之前定义--
	Prompt = "PromptCtrl",
	Message = "MessageCtrl",
	--Sample--
	Sample = "SampleCtrl",
	--Menu--
	Menu = "MenuCtrl"
}

PanelNames = {
	--之前定义--
	"PromptPanel",	
	"MessagePanel",
	--Sample--
	"SamplePanel",
	--Menu--
	"MenuPanel"
}

--协议类型--
ProtocalType = {
	BINARY = 0,
	PB_LUA = 1,
	PBC = 2,
	SPROTO = 3,
}
--当前使用的协议类型--
TestProtoType = ProtocalType.BINARY;

Util = LuaFramework.Util;
AppConst = LuaFramework.AppConst;
LuaHelper = LuaFramework.LuaHelper;
ByteBuffer = LuaFramework.ByteBuffer;

resMgr = LuaHelper.GetResManager();
panelMgr = LuaHelper.GetPanelManager();
soundMgr = LuaHelper.GetSoundManager();
networkMgr = LuaHelper.GetNetManager();

WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;