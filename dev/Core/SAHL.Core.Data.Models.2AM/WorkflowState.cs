using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowStateDataModel :  IDataModel
    {
        public WorkflowStateDataModel(int workflowStateKey, int originationSourceProductKey, string description)
        {
            this.WorkflowStateKey = workflowStateKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Description = description;
		
        }		

        public int WorkflowStateKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public string Description { get; set; }
    }
}