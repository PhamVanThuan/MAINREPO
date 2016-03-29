using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkflowAssignmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowAssignmentDataModel(long instanceID, int offerRoleTypeOrganisationStructureMappingKey, int aDUserKey, int generalStatusKey, DateTime? insertDate, string stateName)
        {
            this.InstanceID = instanceID;
            this.OfferRoleTypeOrganisationStructureMappingKey = offerRoleTypeOrganisationStructureMappingKey;
            this.ADUserKey = aDUserKey;
            this.GeneralStatusKey = generalStatusKey;
            this.InsertDate = insertDate;
            this.StateName = stateName;
		
        }
		[JsonConstructor]
        public WorkflowAssignmentDataModel(int iD, long instanceID, int offerRoleTypeOrganisationStructureMappingKey, int aDUserKey, int generalStatusKey, DateTime? insertDate, string stateName)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
            this.OfferRoleTypeOrganisationStructureMappingKey = offerRoleTypeOrganisationStructureMappingKey;
            this.ADUserKey = aDUserKey;
            this.GeneralStatusKey = generalStatusKey;
            this.InsertDate = insertDate;
            this.StateName = stateName;
		
        }		

        public int ID { get; set; }

        public long InstanceID { get; set; }

        public int OfferRoleTypeOrganisationStructureMappingKey { get; set; }

        public int ADUserKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime? InsertDate { get; set; }

        public string StateName { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}