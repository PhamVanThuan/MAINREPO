using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserOrganisationStructureCapabilityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserOrganisationStructureCapabilityDataModel(int userOrganisationStructureKey, int capabilityKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.CapabilityKey = capabilityKey;
		
        }
		[JsonConstructor]
        public UserOrganisationStructureCapabilityDataModel(int userOrganisationStructureCapabilityKey, int userOrganisationStructureKey, int capabilityKey)
        {
            this.UserOrganisationStructureCapabilityKey = userOrganisationStructureCapabilityKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.CapabilityKey = capabilityKey;
		
        }		

        public int UserOrganisationStructureCapabilityKey { get; set; }

        public int UserOrganisationStructureKey { get; set; }

        public int CapabilityKey { get; set; }

        public void SetKey(int key)
        {
            this.UserOrganisationStructureCapabilityKey =  key;
        }
    }
}