//According to the Json file automatically generated structures 
//Date : 3/21/2016 2:25:59 PM

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Vo
{

    public class WorktableStatus
    {
        public int userId { get; private set; }
        public int typeId { get; private set; }
        public int status { get; private set; }
        public Dictionary<int, int> itemRtime { get; private set; }
    }

}
