//According to the Json file automatically generated structures 
//Date : 3/18/2016 1:25:14 PM

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Vo
{

    public class RobotSkill
    {
        public int userId { get; private set; }
        public int skillId { get; private set; }
        public List<int> progress { get; private set; }
    }

}
