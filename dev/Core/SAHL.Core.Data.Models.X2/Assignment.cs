using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class AssignmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AssignmentDataModel(long instanceId, DateTime assignmentDate, int userOrganisationStructureKey, int capabilityKey)
        {
            this.InstanceId = instanceId;
            this.AssignmentDate = assignmentDate;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.CapabilityKey = capabilityKey;
		
        }
		[JsonConstructor]
        public AssignmentDataModel(int iD, long instanceId, DateTime assignmentDate, int userOrganisationStructureKey, int capabilityKey)
        {
            this.ID = iD;
            this.InstanceId = instanceId;
            this.AssignmentDate = assignmentDate;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.CapabilityKey = capabilityKey;
		
        }		

        public int ID { get; set; }

        public long InstanceId { get; set; }

        public DateTime AssignmentDate { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public int CapabilityKey { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}