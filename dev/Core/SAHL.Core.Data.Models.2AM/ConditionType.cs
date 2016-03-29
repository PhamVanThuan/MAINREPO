using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ConditionTypeDataModel(int conditionTypeKey, string description)
        {
            this.ConditionTypeKey = conditionTypeKey;
            this.Description = description;
		
        }		

        public int ConditionTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ConditionTypeKey =  key;
        }
    }
}