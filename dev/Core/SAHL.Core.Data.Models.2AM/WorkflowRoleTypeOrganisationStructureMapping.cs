using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowRoleTypeOrganisationStructureMappingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowRoleTypeOrganisationStructureMappingDataModel(int workflowRoleTypeKey, int organisationStructureKey)
        {
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }
		[JsonConstructor]
        public WorkflowRoleTypeOrganisationStructureMappingDataModel(int workflowRoleTypeOrganisationStructureMappingKey, int workflowRoleTypeKey, int organisationStructureKey)
        {
            this.WorkflowRoleTypeOrganisationStructureMappingKey = workflowRoleTypeOrganisationStructureMappingKey;
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }		

        public int WorkflowRoleTypeOrganisationStructureMappingKey { get; set; }

        public int WorkflowRoleTypeKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.WorkflowRoleTypeOrganisationStructureMappingKey =  key;
        }
    }
}