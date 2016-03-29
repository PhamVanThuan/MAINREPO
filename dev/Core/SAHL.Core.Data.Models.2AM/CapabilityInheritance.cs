using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapabilityInheritanceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapabilityInheritanceDataModel(int userOrganisationStructureCapabilityKey, int organisationStructureKey)
        {
            this.UserOrganisationStructureCapabilityKey = userOrganisationStructureCapabilityKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }
		[JsonConstructor]
        public CapabilityInheritanceDataModel(int capabilityInheritanceKey, int userOrganisationStructureCapabilityKey, int organisationStructureKey)
        {
            this.CapabilityInheritanceKey = capabilityInheritanceKey;
            this.UserOrganisationStructureCapabilityKey = userOrganisationStructureCapabilityKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }		

        public int CapabilityInheritanceKey { get; set; }

        public int UserOrganisationStructureCapabilityKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.CapabilityInheritanceKey =  key;
        }
    }
}