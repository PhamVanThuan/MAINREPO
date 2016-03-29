using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowRoleTypeGroupDataModel :  IDataModel
    {
        public WorkflowRoleTypeGroupDataModel(int workflowRoleTypeGroupKey, string description, int genericKeyTypeKey, int workflowOrganisationStructureKey)
        {
            this.WorkflowRoleTypeGroupKey = workflowRoleTypeGroupKey;
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.WorkflowOrganisationStructureKey = workflowOrganisationStructureKey;
		
        }		

        public int WorkflowRoleTypeGroupKey { get; set; }

        public string Description { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int WorkflowOrganisationStructureKey { get; set; }
    }
}