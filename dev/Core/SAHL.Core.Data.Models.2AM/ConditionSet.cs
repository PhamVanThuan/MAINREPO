using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionSetDataModel :  IDataModel
    {
        public ConditionSetDataModel(int conditionSetKey, string description)
        {
            this.ConditionSetKey = conditionSetKey;
            this.Description = description;
		
        }		

        public int ConditionSetKey { get; set; }

        public string Description { get; set; }
    }
}