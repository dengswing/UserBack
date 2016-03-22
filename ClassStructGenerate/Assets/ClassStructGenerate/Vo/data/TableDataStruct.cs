//According to the Json file automatically generated structures
//Date : 3/22/2016 4:52:48 PM

using Vo;
using Networks.parser;

namespace Networks
{

    public class TableDataStruct : AbsTableDataStruct
    {

        Networks.DataTableUpdateDelegate generalResponse;

        public override void RegisterBindingTableStrcut()
        {
            typeDict[TableDataNames.ItemSore] = typeof(ItemSore);
            typeDict[TableDataNames.List] = typeof(List);
            typeDict[TableDataNames.Payment] = typeof(Payment);
            typeDict[TableDataNames.ProductComposeHistory] = typeof(ProductComposeHistory);
            typeDict[TableDataNames.RobotInfo] = typeof(RobotInfo);
            typeDict[TableDataNames.RobotSkill] = typeof(RobotSkill);
            typeDict[TableDataNames.WorktableStatus] = typeof(WorktableStatus);
        }

        public void AddGeneralResponse(Networks.DataTableUpdateDelegate response)
        {
            generalResponse -= response;
            generalResponse += response;
        }

        public void RemoveGeneralResponse(Networks.DataTableUpdateDelegate response)
        {
            generalResponse -= response;
        }

        public override void FireNotice(string tableName, object data)
        {
            if (generalResponse != null)
            {
                generalResponse(data);
            }
        }

    }

}
