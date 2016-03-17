//According to the Json file automatically generated structures 
//Date : 3/17/2016 12:34:43 PM

using System;
using System.Collections.Generic;
    public class Item
    {
        public string itemDefId;
        public int? count;
    }

    public class Progress
    {
        public List<Item> item;
    }

    public class RobotInfo
    {
        public int? userId;
        public int? moduleId;
        public int? moduleLv;
        public Progress progress;
        public int? isUnlock;
        public int? proficiency;
        public List<int> unlockSkill;
    }

