using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowRuleSetItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowRuleSetItemDataModel(int workflowRuleSetKey, int ruleItemKey)
        {
            this.WorkflowRuleSetKey = workflowRuleSetKey;
            this.RuleItemKey = ruleItemKey;
		
        }
		[JsonConstructor]
        public WorkflowRuleSetItemDataModel(int workflowRuleSetItemKey, int workflowRuleSetKey, int ruleItemKey)
        {
            this.WorkflowRuleSetItemKey = workflowRuleSetItemKey;
            this.WorkflowRuleSetKey = workflowRuleSetKey;
            this.RuleItemKey = ruleItemKey;
		
        }		

        public int WorkflowRuleSetItemKey { get; set; }

        public int WorkflowRuleSetKey { get; set; }

        public int RuleItemKey { get; set; }

        public void SetKey(int key)
        {
            this.WorkflowRuleSetItemKey =  key;
        }
    }
}