using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RuleExclusionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public RuleExclusionDataModel(int ruleExclusionSetKey, int ruleItemKey)
        {
            this.RuleExclusionSetKey = ruleExclusionSetKey;
            this.RuleItemKey = ruleItemKey;
		
        }
		[JsonConstructor]
        public RuleExclusionDataModel(int ruleExclusionKey, int ruleExclusionSetKey, int ruleItemKey)
        {
            this.RuleExclusionKey = ruleExclusionKey;
            this.RuleExclusionSetKey = ruleExclusionSetKey;
            this.RuleItemKey = ruleItemKey;
		
        }		

        public int RuleExclusionKey { get; set; }

        public int RuleExclusionSetKey { get; set; }

        public int RuleItemKey { get; set; }

        public void SetKey(int key)
        {
            this.RuleExclusionKey =  key;
        }
    }
}