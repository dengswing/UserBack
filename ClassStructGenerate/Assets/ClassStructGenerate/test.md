角色信息系统设计
================

维护人 | 版本号 | 描述 | 日期
--- | --- | ---
金非非 | 0.1 | 初版 | 2015-09-28
金非非 | 0.2 | 新增熟练度,客户端解锁玩法tips | 2015-11-25
金非非 | 0.3 | 新增初次解锁和技能学习 | 2016-01-06
金非非 | 0.4 | 修改解锁条件，升级条件，技能学习条件 | 2016-02-23

## 1. 设计   
### 1.1 概念
* 玩家通过修复或者升级不同模块来解锁相应功能，机器人芯片一开始状态为修复，之后状态为升级。
* 当修复该模块后，再次点击该模块会出现下一个等级的模块所需求的物品和熟练度。
* 各个模块有对应的熟练度,当对应的熟练度和所需物品数量达到后可以升级对应模块等级。
* 学习技能后可能会解锁一些功能。例如击球小游戏，滑板小游戏等。

## 2. 配置表设计
### 2.1 机器人芯片配置表

配置表名称：sg_robot_info   
配置表路径 : config/en_US  

配置表项：

字段名 | 类型 | 默认值 | 使用者 | 索引 | 描述 
--- | --- | --- | --- 
moduleUniqueId | int | - | both | pri | 模块组合Id
name | string | - | client | - | 模块名称
upgradeCondition | json | - | both | - | 升级所需物品 e.g:2.1.c1
upgradeProficiency | int | - | both | - | 升级所需的总熟练度
rateEffect | json | - | both | - | 熟练度影响的概率系数 e.g:2.1.c2 
nextUniqueId | int | - | both | - | 下一模块组合Id
rewardExp | int | - | both | - | 升级获取的经验值
message | string | - | client | - | 升级成功提示语   
icon | string | - | client | - | 图标资源    
iconRobot | string | - | client | - | 机器人图片资源
description | string | - | client | - | 描述

模块组合Id为模块Id和模块等级组成   
芯片解锁，各个模块不同等级解锁，学习技能后解锁功能，请看`功能解锁系统设计`。

### 2.3 机器人技能树
配置表名称：sg_robot_skill  
配置表路径 : config/en_US  

配置表项：

字段名 | 类型 | 默认值 | 使用者 | 索引 | 描述 
--- | --- | --- | ---  
skillDefId | int | - | both | - | 技能DefId
moduleId | int | - | both | - | 模块Id
target | json | - | both | - | 条件 e.g:2.1.c1
targetDesc | json | - | client | - | 条件描述 e.g：['啊啊啊啊','噢噢噢噢','嗯嗯嗯嗯']
targetIcon | json | - | client | - | 条件图标
consume | json | - | both | - | 学习消耗
icon | int | - | client | - | 图标
desc | string | - | client | - | 描述
name | string | - | client | - | 技能名

<code>
2.1.c1

	[
        {entityId:count},
        ...
    ]

<code>
2.1.c2

    [
        {"proficiency":0.1,"sideEffect":0.4},
        {"proficiency":0.3,"sideEffect":0.5},
        ...
    ]

<code>
2.1.c3

	[FunctionDefId,FunctionDefId,FunctionDefId...]

## 3. 存储结构
### 3.1 单个模块信息
表名：sg_robot_info

字段名	 | 类型 | 默认值 | 索引 | 备注 
--- | --- | --- | --- | ---
userId | float | - | pri | 玩家用户id
moduleId | long | - | pri | 模块Id
moduleLv | int | - | - | 模块当前等级
progress | json | - | - | 模块升级进度 e.g:2.1.c1
isUnlock | int | - | - | 是否模块已解锁 可以则为1，不能则为0
proficiency | int | - | - | 模块熟练度 
unlockSkill | json | - | - | 已解锁的技能 e.g:2.1.c4

<code>
2.1.c1

	
      [ 
         {"itemDefId": 10000003,"cd": 35, "completeTime": 1448872816, 
			"material": [{"10010011": 1},{"10010012": 1}]
		 }, 
		{"itemDefId": 10000011,"cd": 35,"completeTime": 1448872851,
		"material": [ {"10010008": 3}, {"10010020": 1}, {"10010023": 1}]  
         } 
      ]

    
<code>
2.1.c4

	[1,2,3]

### 3.2 技能进度
表名:sg_robot_skill

字段名	 | 类型 | 默认值 | 索引 | 备注 
--- | --- | --- | --- | ---
userId | int | - | pri | 玩家用户id
skillId | int | - | pri | 技能Id
progress | json | - | - | 进度 e.g:[1,2,3]

## 4.API
### 4.1 修复
接口名：RobotInfo.repair   
输入：接口必要参数及

参数名 | 类型 | 默认值 | 备注
--- | --- | --- | ---
moduleUniqueId | int | - | 模块组合Id
输出：Msg结构中输出布尔值，Update输出 RobotInfo模块Vo模型

### 4.2 记录学习的技能
接口名:robotInfo.learnSkill
输入：接口必要参数及  

参数名 | 类型 | 默认值 | 备注  
--- | --- | --- | ---  
skillDefId | int | - | 技能Id  
输出：Msg结构中输出布尔值，Update输出 RobotInfo模块Vo模型

解锁或者目标更新，update会返回RobotSkill模块的VoList模型