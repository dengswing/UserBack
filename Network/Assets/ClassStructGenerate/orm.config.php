<?php
return array(
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- PRODUCTION HANDLING
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'List' => array(
        'table' => 'sg_list',
        'columns' => array('userId', 'type', 'listInfo'),
        'autoIncrColumn' => null,
        'diffUpColumns' => array(),
        'updateFilter' => array(0, 1),
        'toArrayFilter' => array(0),
        'searchColumns' => array(0, 1),
        'updateColumns' => array(0, 1),
        'jsonColumns' => array(2=>4000),
        'cacheColumn' => 0,
        'shardColumn' => 0,
        'pkColumn' => 1,
        'tableShardCount' => null,
        'deleteColumns' => array(0, 1),
        'isList' => true,
        'dbType' => 'MySql',
        'cacheTime' => 604800, // 7 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- PRODUCTION COMPOSE HISTORY
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'ProductComposeHistory' => array(
        'table' => 'sg_product_compose_history',
        'columns' => array('userId', 'productDefId', 'count', 'updateTime'),
        'autoIncrColumn' => null,
        'diffUpColumns' => array(2),
        'updateFilter' => array(0, 1),
        'toArrayFilter' => array(),
        'searchColumns' => array(0, 1),
        'updateColumns' => array(0, 1),
        'jsonColumns' => array(),
        'cacheColumn' => 0,
        'shardColumn' => 0,
        'pkColumn' => 1,
        'tableShardCount' => null,
        'deleteColumns' => array(0, 1),
        'isList' => true,
        'dbType' => 'MySql',
        'cacheTime' => 604800, // 7 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Skill
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Skill' => array(
        'table' => 'sg_skill',
        'columns' => array('userId', 'skillDefId', 'level', 'status', 'updateTime'),
        'autoIncrColumn' => null,
        'diffUpColumns' => array(2),
        'updateFilter' => array(0, 1),
        'toArrayFilter' => array(),
        'searchColumns' => array(0, 1),
        'updateColumns' => array(0, 1),
        'jsonColumns' => array(),
        'cacheColumn' => 0,
        'shardColumn' => 0,
        'pkColumn' => 1,
        'tableShardCount' => null,
        'deleteColumns' => array(0, 1),
        'isList' => true,
        'dbType' => 'MySql',
        'cacheTime' => 604800, // 7 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- SkillProgress
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'SkillProgress' => array(
        'table' => 'sg_skill_progress',
        'columns' => array('userId', 'skillDefId', 'type', 'createTime'),
        'autoIncrColumn' => null,
        'diffUpColumns' => array(),
        'updateFilter' => array(0),
        'toArrayFilter' => array(),
        'searchColumns' => array(0),
        'updateColumns' => array(0),
        'jsonColumns' => array(),
        'cacheColumn' => 0,
        'shardColumn' => 0,
        'pkColumn' => 0,
        'tableShardCount' => null,
        'deleteColumns' => array(0),
        'isList' => false,
        'dbType' => 'MySql',
        'cacheTime' => 604800, // 7 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- OldElectricNpc
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'OldElectricNpc' => array(
        'table' => 'sg_old_electric_npc',
        'columns' => array('electricNpcId', 'userId', 'electricNpcDefId', 'carryOldElectricDefId', 'lastLeaveTime'),
        'autoIncrColumn' => 0,
        'diffUpColumns' => array(),
        'updateFilter' => array(0, 1),
        'toArrayFilter' => array(),
        'searchColumns' => array(0, 1),
        'updateColumns' => array(0, 1),
        'jsonColumns' => array(),
        'cacheColumn' => 1,
        'shardColumn' => 1,
        'pkColumn' => 0,
        'tableShardCount' => null,
        'deleteColumns' => array(0, 1),
        'isList' => true,
        'dbType' => 'MySql',
        'cacheTime' => 604800, // 7 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Event
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Event' => array(
        'table' => 'sg_event',
        'columns' => array('userId', 'eventDefId', 'createTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(2),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- EventHistory
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'EventHistory' => array(
        'table' => 'sg_event_history',
        'columns' => array('userId', 'eventDefId', 'count', 'rewards', 'completeTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(2),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>1000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- EventProgress
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'EventProgress' => array(
        'table' => 'sg_event_in_progress',
        'columns' => array('userId', 'rowShard', 'progress'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(1),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2 => 5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Building
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Building' => array(
        'table' => 'sg_building',
        'columns' => array('buildingId', 'userId', 'buildingDefId', 'groundDefId', 'level', 'status', 'createTime', 'updateTime'),
        'autoIncrColumn' => 0,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 1,
        'shardColumn'    => 1,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Buildings
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Buildings' => array(
        'table' => 'sg_buildings',
        'columns' => array('userId', 'type', 'count'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(2), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- ground
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Ground' => array(
        'table' => 'sg_ground',
        'columns' => array('userId', 'cityDefId', 'grounds'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0,1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0,1),
        'updateColumns'  => array(0,1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0,1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- City
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'City' => array(
        'table' => 'sg_city',
        'columns' => array('userId', 'stateDefId', 'cities'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- State
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'State' => array(
        'table' => 'sg_state',
        'columns' => array('userId', 'states'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- City Order
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CityOrder' => array(
        'table' => 'sg_city_order',
        'columns' => array('userId', 'cityOrderDefId', 'isRemove', 'completeTime', 'sendComplateTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- City Order
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CityOrderHistory' => array(
        'table' => 'sg_city_order_history',
        'columns' => array('orderId', 'userId', 'cityOrderDefId', 'isRemove', 'completeTime', 'sendComplateTime'),
        'autoIncrColumn' => 0,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 1,
        'shardColumn'    => 1,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MetalsSearch
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MetalsSearch' => array(
        'table' => 'sg_metals_search_result',
        'columns' => array('userId', 'pos_1', 'pos_2', 'pos_3'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>100, 2=>100, 3=>100),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MissionLast
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MissionLast' => array(
        'table'          => 'sg_mission_last',
        'columns'        => array('userId', 'type', 'missionId'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MissionUnlock
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MissionUnlock' => array(
        'table'          => 'sg_mission_unlock',
        'columns'        => array('userId', 'mission'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MissionGiveUp
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MissionGiveUp' => array(
        'table'          => 'sg_mission_giveup',
        'columns'        => array('userId', 'rowShard', 'giveUp'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2 => 5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     //- CityMaps
     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     'CityMaps' => array(
         'table' => 'sg_city_maps',
         'columns' => array('userId', 'cityDefId', 'block_dict', 'build_dict'),
         'autoIncrColumn' => null,
         'diffUpColumns'  => array(), // difference update columns
         'updateFilter'   => array(0, 1),
         'toArrayFilter'  => array(0),
         'searchColumns'  => array(0, 1),
         'updateColumns'  => array(0, 1),
         'jsonColumns'    => array(2=>5000, 3=>5000),
         'cacheColumn'    => 0,
         'shardColumn'    => 0,
         'pkColumn'       => 1,
         'deleteColumns'  => array(0, 1),
         'tableShardCount'=> null,
         'isList'         => true,
         'dbType'         => 'MySql',
         'cacheTime'      => 259200, // 3 days
     ),

     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     //- BuildingProgress
     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     'BuildingProgress' => array(
         'table' => 'sg_building_progress',
         'columns' => array('progressId', 'userId', 'groundDefId', 'buildingDefId', 'completeTime'),
         'autoIncrColumn' => 0,
         'diffUpColumns' => array(),
         'updateFilter' => array(0, 1),
         'toArrayFilter' => array(),
         'searchColumns' => array(0, 1),
         'updateColumns' => array(0, 1),
         'jsonColumns' => array(),
         'cacheColumn' => 1,
         'shardColumn' => 1,
         'pkColumn' => 0,
         'tableShardCount' => null,
         'deleteColumns' => array(0, 1),
         'isList' => true,
         'dbType' => 'MySql',
         'cacheTime' => 259200, // 7 days
     ),

     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     //- TakeOverProgress
     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     'TakeOverProgress' => array(
         'table' => 'sg_take_over_progress',
         'columns' => array('id', 'userId', 'cityDefId', 'buildDictIndex', 'completeTime'),
         'autoIncrColumn' => 0,
         'diffUpColumns' => array(),
         'updateFilter' => array(0, 1),
         'toArrayFilter' => array(),
         'searchColumns' => array(0, 1),
         'updateColumns' => array(0, 1),
         'jsonColumns' => array(),
         'cacheColumn' => 1,
         'shardColumn' => 1,
         'pkColumn' => 0,
         'tableShardCount' => null,
         'deleteColumns' => array(0, 1),
         'isList' => true,
         'dbType' => 'MySql',
         'cacheTime' => 259200, // 7 days
     ),

     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     //- BlockExtendProgress
     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     'BlockExtendProgress' => array(
         'table' => 'sg_block_extend_progress',
         'columns' => array('id', 'userId', 'cityDefId', 'blockDefId', 'completeTime'),
         'autoIncrColumn' => 0,
         'diffUpColumns' => array(),
         'updateFilter' => array(0, 1),
         'toArrayFilter' => array(),
         'searchColumns' => array(0, 1),
         'updateColumns' => array(0, 1),
         'jsonColumns' => array(),
         'cacheColumn' => 1,
         'shardColumn' => 1,
         'pkColumn' => 0,
         'tableShardCount' => null,
         'deleteColumns' => array(0, 1),
         'isList' => true,
         'dbType' => 'MySql',
         'cacheTime' => 259200, // 7 days
     ),

     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     //- BlockExtendProgress
     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     'BuildingUpgradeProgress' => array(
         'table' => 'sg_building_upgrade_progress',
         'columns' => array('id', 'userId', 'cityDefId', 'buildDictIndex', 'completeTime'),
         'autoIncrColumn' => 0,
         'diffUpColumns' => array(),
         'updateFilter' => array(0, 1),
         'toArrayFilter' => array(),
         'searchColumns' => array(0, 1),
         'updateColumns' => array(0, 1),
         'jsonColumns' => array(),
         'cacheColumn' => 1,
         'shardColumn' => 1,
         'pkColumn' => 0,
         'tableShardCount' => null,
         'deleteColumns' => array(0, 1),
         'isList' => true,
         'dbType' => 'MySql',
         'cacheTime' => 259200, // 7 days
     ),

     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     //- StoreJoin
     //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
     'StoreJoin' => array(
         'table' => 'sg_store_join',
         'columns' => array('userId', 'buildingUniqueId', 'currentToken', 'currentSaleDevices', 'currentDevices', 'lastHarvestTime', 'lastAddTime'),
         'autoIncrColumn' => null,
         'diffUpColumns' => array(),
         'updateFilter' => array(0, 1),
         'toArrayFilter' => array(),
         'searchColumns' => array(0, 1),
         'updateColumns' => array(0, 1),
         'jsonColumns' => array(3=>250, 4=>550),
         'cacheColumn' => 0,
         'shardColumn' => 0,
         'pkColumn' => 1,
         'tableShardCount' => null,
         'deleteColumns' => array(0, 1),
         'isList' => true,
         'dbType' => 'MySql',
         'cacheTime' => 259200, // 7 days
     ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- PackageLv
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'PackageLv' => array(
        'table'          => 'module_item_package_lv',
        'columns'        => array('userId', 'packageLv'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Room Order
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'RoomOrder' => array(
        'table' => 'sg_room_order',
        'columns' => array('userId', 'roomOrderDefId', 'isRemove', 'completeTime', 'sendCompleteTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- City Order
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'RoomOrderHistory' => array(
        'table' => 'sg_room_order_history',
        'columns' => array('orderId', 'userId', 'roomOrderDefId', 'isRemove', 'completeTime', 'sendCompleteTime'),
        'autoIncrColumn' => 0,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 1,
        'shardColumn'    => 1,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Robot
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'RobotInfo' => array(
        'table'          => 'sg_robot_info',
        'columns'        => array('userId', 'moduleId', 'moduleLv', 'progress', 'proficiency', 'isUnlock', 'unlockSkill', 'reProgress'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(3 => 512, 6 => 2000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Knowledge
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Knowledge' => array(
        'table'          => 'sg_knowledge',
        'columns'        => array('userId', 'knowledgeDefId', 'count', 'isNew', 'createTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(2),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- ProductFormula
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'ProductFormula' => array(
        'table'          => 'sg_product_formula',
        'columns'        => array('userId', 'formulaDefId', 'isNew', 'createTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(0),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- GarageProduct
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'GarageProduct' => array(
        'table'          => 'sg_garage_product',
        'columns'        => array('productId','userId', 'projectDefId','partDefIds','garbages','score','createTime'),
        'autoIncrColumn' => 0,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(3=>255, 4=>1000),
        'cacheColumn'    => 1,
        'shardColumn'    => 1,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //- CollectionItemInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CollectionItemInfo' => array(
        'table' => 'sg_collection_item',
        'columns' => array('userId', 'pos_1', 'pos_2', 'pos_3', 'pos_4', 'pos_5', 'pos_6', 'pos_7', 'pos_8', 'pos_9', 'pos_10', 'nextFlushTime', 'acquireTime', 'searchItemId'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>100, 2=>100, 3=>100, 4=>100, 5=>100, 6=>100, 7=>100, 8=>100, 9=>100, 10=>100),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- BranchOrder
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'BranchOrder' => array(
        'table' => 'sg_branch_order',
        'columns' => array('branchOrderId', 'userId', 'branchOrderDefId', 'progress', 'isRemove', 'nextOrderTime', 'nextOrderDefId'),
        'autoIncrColumn' => 0,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(3=>500),
        'cacheColumn'    => 1,
        'shardColumn'    => 1,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Question
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Question' => array(
        'table' => 'sg_question_info',
        'columns' => array('userId', 'stageDefId', 'nextQuestionTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0,1),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- QuestionHistory
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'QuestionHistory' => array(
        'table' => 'sg_question_history',
        'columns' => array('userId', 'stageDefId', 'historyQuestionId'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0,1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- ScanInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'ScanInfo' => array(
        'table' => 'sg_scan_info',
        'columns' => array('userId', 'scanDefId', 'nextScanTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(), // difference update columns
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0,1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MiniGameInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MiniGameInfo' => array(
        'table'          => 'sg_mini_game_info',
        'columns'        => array('userId', 'infoType', 'decreaseDefId', 'record', 'dailyExp', 'dailyProficiency'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- SkateboardReward
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'SkateboardReward' => array(
        'table'          => 'sg_skateboard_reward',
        'columns'        => array('userId', 'exp', 'proficiency'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- CollectionSearchList
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CollectionSearchList' => array(
        'table'          => 'sg_collection_search_list',
        'columns'        => array('userId', 'searchs'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- CollectionSearchInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CollectionSearchInfo' => array(
        'table'          => 'sg_collection_search',
        'columns'        => array('userId', 'searchItem'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>1000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- SupplierInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'SupplierInfo' => array(
        'table'          => 'sg_supplier_info',
        'columns'        => array('userId', 'npcId', 'itemDefId', 'flushDate', 'acquireNum'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- SupplierList
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'SupplierList' => array(
        'table'          => 'sg_supplier_list',
        'columns'        => array('userId', 'npcId', 'item'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>10000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- DeviceAssemble
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'DeviceAssemble' => array(
        'table'          => 'sg_assemble_device',
        'columns'        => array('userId','deviceDefId','parts'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>80),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- DeviceAssemble
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'DeviceOutAssemble' => array(
        'table'          => 'sg_out_assemble_device',
        'columns'        => array('userId', 'pos','deviceDefId', 'createTime', 'leaveTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- TelevisionState
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'TelevisionState' => array(
        'table'          => 'sg_television_state',
        'columns'        => array('userId', 'state', 'watchTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- DialogueFinished
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'DialogueFinished' => array(
        'table'          => 'sg_dialogue_finished',
        'columns'        => array('userId', 'rowShard', 'dialogue'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- UnlockProgress
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'UnlockProgress' => array(
        'table'          => 'sg_unlock_progress',
        'columns'        => array('userId', 'unlockId', 'progress'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- UnlockProgress
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'UnlockFunction' => array(
        'table'          => 'sg_unlock_function',
        'columns'        => array('userId', 'rowShard', 'fids'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- BroadcastHistory
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'BroadcastHistory' => array(
        'table'          => 'sg_broadcast_history',
        'columns'        => array('userId', 'rowShard', 'broadcast'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MoodInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MoodInfo' => array(
        'table'          => 'sg_mood_info',
        'columns'        => array('userId', 'sceneInter', 'mood', 'time', 'openTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Newspaper
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Newspaper' => array(
        'table'          => 'sg_newspaper',
        'columns'        => array('userId', 'newspaperDefId', 'isRead', 'createTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- UnlockOutAssemble
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'UnlockOutAssemble' => array(
        'table'          => 'sg_unlock_out_assemble',
        'columns'        => array('userId', 'rowShard', 'unlocks'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>5000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MoodGame
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MoodGame' => array(
        'table'          => 'sg_mood_game',
        'columns'        => array('userId', 'gameDefId', 'isUnlock', 'anime', 'playAnime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(3=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- StatusInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'StatusInfo' => array(
        'table'          => 'sg_status_info',
        'columns'        => array('userId', 'mood', 'moodTime', 'status', 'changeTime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- StudyInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'StudyInfo' => array(
        'table'          => 'sg_study_info',
        'columns'        => array('userId', 'studyDefId', 'finishTime', 'cd'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- RandomDisassemble
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'RandomDisassemble' => array(
        'table'          => 'sg_random_disassemble',
        'columns'        => array('userId', 'disassembleDefId', 'randomTime', 'state'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- MoodInter
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'MoodInter' => array(
        'table'          => 'sg_mood_interaction',
        'columns'        => array('userId', 'interDefId', 'time'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- PartInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'PartInfo' => array(
        'table'          => 'sg_part_info',
        'columns'        => array('userId', 'locationId'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>100),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- CabinetInfo
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CabinetInfo' => array(
        'table'          => 'sg_cabinet_info',
        'columns'        => array('userId', 'grids', 'devices', 'decorations'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0),
        'updateColumns'  => array(0),
        'jsonColumns'    => array(1=>200, 2=>500, 3=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0),
        'tableShardCount'=> null,
        'isList'         => false,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- Customer
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'Customer' => array(
        'table'          => 'sg_customer',
        'columns'        => array('customerId', 'userId', 'group', 'skillDefId', 'deviceDefId', 'isInStore'),
        'autoIncrColumn' => 0,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(),
        'cacheColumn'    => 1,
        'shardColumn'    => 1,
        'pkColumn'       => 0,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- CustomerGroup
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'CustomerGroup' => array(
        'table'          => 'sg_customer_group',
        'columns'        => array('userId', 'group', 'acceptance', 'skills'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(3=>200),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- RobotSkill
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'RobotSkill' => array(
        'table'          => 'sg_robot_skill',
        'columns'        => array('userId', 'skillId', 'progress'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- RobotSkill
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'WorktableStatus' => array(
        'table'          => 'sg_worktable_status',
        'columns'        => array('userId', 'typeId', 'status', 'itemRtime', 'itemCtime'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(3=>2000, 4=>2000),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //- ProjectProduct
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    'ProjectProduct' => array(
        'table'          => 'sg_project_product',
        'columns'        => array('userId', 'projectDefId', 'partDefIds', 'score'),
        'autoIncrColumn' => null,
        'diffUpColumns'  => array(),
        'updateFilter'   => array(0, 1),
        'toArrayFilter'  => array(),
        'searchColumns'  => array(0, 1),
        'updateColumns'  => array(0, 1),
        'jsonColumns'    => array(2=>500),
        'cacheColumn'    => 0,
        'shardColumn'    => 0,
        'pkColumn'       => 1,
        'deleteColumns'  => array(0, 1),
        'tableShardCount'=> null,
        'isList'         => true,
        'dbType'         => 'MySql',
        'cacheTime'      => 259200, // 3 days
    ),
);
