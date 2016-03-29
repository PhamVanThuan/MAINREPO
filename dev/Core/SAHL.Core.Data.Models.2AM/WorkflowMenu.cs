using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowMenuDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowMenuDataModel(string workflowName, string stateName, int coreBusinessObjectKey, string processName)
        {
            this.WorkflowName = workflowName;
            this.StateName = stateName;
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.ProcessName = processName;
		
        }
		[JsonConstructor]
        public WorkflowMenuDataModel(int workflowMenuKey, string workflowName, string stateName, int coreBusinessObjectKey, string processName)
        {
            this.WorkflowMenuKey = workflowMenuKey;
            this.WorkflowName = workflowName;
            this.StateName = stateName;
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.ProcessName = processName;
		
        }		

        public int WorkflowMenuKey { get; set; }

        public string WorkflowName { get; set; }

        public string StateName { get; set; }

        public int CoreBusinessObjectKey { get; set; }

        public string ProcessName { get; set; }

        public void SetKey(int key)
        {
            this.WorkflowMenuKey =  key;
        }
    }
}