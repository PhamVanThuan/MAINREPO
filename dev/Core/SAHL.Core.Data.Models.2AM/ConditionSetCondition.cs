using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionSetConditionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionSetConditionDataModel(int conditionSetKey, int conditionKey, bool requiredCondition)
        {
            this.ConditionSetKey = conditionSetKey;
            this.ConditionKey = conditionKey;
            this.RequiredCondition = requiredCondition;
		
        }
		[JsonConstructor]
        public ConditionSetConditionDataModel(int conditionSetConditionKey, int conditionSetKey, int conditionKey, bool requiredCondition)
        {
            this.ConditionSetConditionKey = conditionSetConditionKey;
            this.ConditionSetKey = conditionSetKey;
            this.ConditionKey = conditionKey;
            this.RequiredCondition = requiredCondition;
		
        }		

        public int ConditionSetConditionKey { get; set; }

        public int ConditionSetKey { get; set; }

        public int ConditionKey { get; set; }

        public bool RequiredCondition { get; set; }

        public void SetKey(int key)
        {
            this.ConditionSetConditionKey =  key;
        }
    }
}