//According to the Json file automatically generated structures 
//Date : 3/17/2016 8:38:02 PM

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Vo
{

    public class Item
    {
        public int s { get; private set; }
    }

    public class K
    {
        public string s { get; private set; }
    }

    public class Progress
    {
        public Item item { get; private set; }
        public K k { get; private set; }
    }

    public class RobotInfo
    {
        public int userId { get; private set; }
        public int moduleId { get; private set; }
        public int moduleLv { get; private set; }
        public List<Progress> progress { get; private set; }
        public int isUnlock { get; private set; }
        public int proficiency { get; private set; }
        public List<int> unlockSkill { get; private set; }
    }

}
