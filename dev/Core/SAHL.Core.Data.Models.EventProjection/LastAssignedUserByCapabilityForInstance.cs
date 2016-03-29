using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class LastAssignedUserByCapabilityForInstanceDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public LastAssignedUserByCapabilityForInstanceDataModel(DateTime lastUpdated, long instanceId, int capabilityKey, int userOrganisationStructureKey, int genericKeyTypeKey, int genericKey, string userName)
        {
            this.LastUpdated = lastUpdated;
            this.InstanceId = instanceId;
            this.CapabilityKey = capabilityKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.UserName = userName;
		
        }
		[JsonConstructor]
        public LastAssignedUserByCapabilityForInstanceDataModel(int id, DateTime lastUpdated, long instanceId, int capabilityKey, int userOrganisationStructureKey, int genericKeyTypeKey, int genericKey, string userName)
        {
            this.Id = id;
            this.LastUpdated = lastUpdated;
            this.InstanceId = instanceId;
            this.CapabilityKey = capabilityKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.UserName = userName;
		
        }		

        public int Id { get; set; }

        public DateTime LastUpdated { get; set; }

        public long InstanceId { get; set; }

        public int CapabilityKey { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public string UserName { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}