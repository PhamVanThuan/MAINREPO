using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionMappingTEMPDataModel :  IDataModel
    {
        public ConditionMappingTEMPDataModel(int? conditionNumber, int? conditionKey)
        {
            this.ConditionNumber = conditionNumber;
            this.ConditionKey = conditionKey;
		
        }		

        public int? ConditionNumber { get; set; }

        public int? ConditionKey { get; set; }
    }
}