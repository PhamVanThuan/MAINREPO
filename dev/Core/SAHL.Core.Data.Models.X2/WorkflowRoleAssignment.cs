using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkflowRoleAssignmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowRoleAssignmentDataModel(long instanceID, int workflowRoleTypeOrganisationStructureMappingKey, int aDUserKey, int generalStatusKey, DateTime? insertDate, string message)
        {
            this.InstanceID = instanceID;
            this.WorkflowRoleTypeOrganisationStructureMappingKey = workflowRoleTypeOrganisationStructureMappingKey;
            this.ADUserKey = aDUserKey;
            this.GeneralStatusKey = generalStatusKey;
            this.InsertDate = insertDate;
            this.Message = message;
		
        }
		[JsonConstructor]
        public WorkflowRoleAssignmentDataModel(int iD, long instanceID, int workflowRoleTypeOrganisationStructureMappingKey, int aDUserKey, int generalStatusKey, DateTime? insertDate, string message)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
            this.WorkflowRoleTypeOrganisationStructureMappingKey = workflowRoleTypeOrganisationStructureMappingKey;
            this.ADUserKey = aDUserKey;
            this.GeneralStatusKey = generalStatusKey;
            this.InsertDate = insertDate;
            this.Message = message;
		
        }		

        public int ID { get; set; }

        public long InstanceID { get; set; }

        public int WorkflowRoleTypeOrganisationStructureMappingKey { get; set; }

        public int ADUserKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime? InsertDate { get; set; }

        public string Message { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}