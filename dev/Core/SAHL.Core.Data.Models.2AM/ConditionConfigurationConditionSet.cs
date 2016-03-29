using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionConfigurationConditionSetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionConfigurationConditionSetDataModel(int conditionConfigurationKey, int conditionSetKey)
        {
            this.ConditionConfigurationKey = conditionConfigurationKey;
            this.ConditionSetKey = conditionSetKey;
		
        }
		[JsonConstructor]
        public ConditionConfigurationConditionSetDataModel(int conditionConfigurationConditionSetKey, int conditionConfigurationKey, int conditionSetKey)
        {
            this.ConditionConfigurationConditionSetKey = conditionConfigurationConditionSetKey;
            this.ConditionConfigurationKey = conditionConfigurationKey;
            this.ConditionSetKey = conditionSetKey;
		
        }		

        public int ConditionConfigurationConditionSetKey { get; set; }

        public int ConditionConfigurationKey { get; set; }

        public int ConditionSetKey { get; set; }

        public void SetKey(int key)
        {
            this.ConditionConfigurationConditionSetKey =  key;
        }
    }
}