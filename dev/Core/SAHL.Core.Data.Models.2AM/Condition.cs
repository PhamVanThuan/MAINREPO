using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionDataModel(int? conditionTypeKey, string conditionPhrase, string tokenDescriptions, int? translatableItemKey, string conditionName)
        {
            this.ConditionTypeKey = conditionTypeKey;
            this.ConditionPhrase = conditionPhrase;
            this.TokenDescriptions = tokenDescriptions;
            this.TranslatableItemKey = translatableItemKey;
            this.ConditionName = conditionName;
		
        }
		[JsonConstructor]
        public ConditionDataModel(int conditionKey, int? conditionTypeKey, string conditionPhrase, string tokenDescriptions, int? translatableItemKey, string conditionName)
        {
            this.ConditionKey = conditionKey;
            this.ConditionTypeKey = conditionTypeKey;
            this.ConditionPhrase = conditionPhrase;
            this.TokenDescriptions = tokenDescriptions;
            this.TranslatableItemKey = translatableItemKey;
            this.ConditionName = conditionName;
		
        }		

        public int ConditionKey { get; set; }

        public int? ConditionTypeKey { get; set; }

        public string ConditionPhrase { get; set; }

        public string TokenDescriptions { get; set; }

        public int? TranslatableItemKey { get; set; }

        public string ConditionName { get; set; }

        public void SetKey(int key)
        {
            this.ConditionKey =  key;
        }
    }
}