using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowOrganisationStructureDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowOrganisationStructureDataModel(int organisationStructureKey, string workflowName, string processName)
        {
            this.OrganisationStructureKey = organisationStructureKey;
            this.WorkflowName = workflowName;
            this.ProcessName = processName;
		
        }
		[JsonConstructor]
        public WorkflowOrganisationStructureDataModel(int workflowOrganisationStructureKey, int organisationStructureKey, string workflowName, string processName)
        {
            this.WorkflowOrganisationStructureKey = workflowOrganisationStructureKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.WorkflowName = workflowName;
            this.ProcessName = processName;
		
        }		

        public int WorkflowOrganisationStructureKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public string WorkflowName { get; set; }

        public string ProcessName { get; set; }

        public void SetKey(int key)
        {
            this.WorkflowOrganisationStructureKey =  key;
        }
    }
}