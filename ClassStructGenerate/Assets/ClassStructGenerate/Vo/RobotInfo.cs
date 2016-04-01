//According to the Json file automatically generated structures 

using System.Collections.Generic;

namespace Vo
{

    public class Progress
    {
        public int itemDefId { get; private set; }
        public int cd { get; private set; }
        public int completeTime { get; private set; }
        public List<Dictionary<float, string>> material { get; private set; }
    }

    public class RobotInfo
    {
        public float userId { get; private set; }
        public long moduleId { get; private set; }
        public int moduleLv { get; private set; }
        public List<Progress> progress { get; private set; }
        public int isUnlock { get; private set; }
        public int proficiency { get; private set; }
        public List<long> unlockSkill { get; private set; }
    }

}
