using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowRoleTypeDataModel :  IDataModel
    {
        public WorkflowRoleTypeDataModel(int workflowRoleTypeKey, string description, int workflowRoleTypeGroupKey)
        {
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
            this.Description = description;
            this.WorkflowRoleTypeGroupKey = workflowRoleTypeGroupKey;
		
        }		

        public int WorkflowRoleTypeKey { get; set; }

        public string Description { get; set; }

        public int WorkflowRoleTypeGroupKey { get; set; }
    }
}