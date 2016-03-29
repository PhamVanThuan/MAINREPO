using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowRuleSetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowRuleSetDataModel(string name)
        {
            this.Name = name;
		
        }
		[JsonConstructor]
        public WorkflowRuleSetDataModel(int workflowRuleSetKey, string name)
        {
            this.WorkflowRuleSetKey = workflowRuleSetKey;
            this.Name = name;
		
        }		

        public int WorkflowRuleSetKey { get; set; }

        public string Name { get; set; }

        public void SetKey(int key)
        {
            this.WorkflowRuleSetKey =  key;
        }
    }
}