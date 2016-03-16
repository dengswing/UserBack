//According to the Json file automatically generated structures 
//Date : 3/16/2016 3:32:12 PM

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

    public class sg_robot_info
    {
        public int? userId;
        public int? moduleId;
        public int? moduleLv;
        public Progress progress;
        public int? isUnlock;
        public int? proficiency;
        public List<int> unlockSkill;
    }

