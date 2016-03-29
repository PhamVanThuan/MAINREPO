using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleTypeOrganisationStructureMappingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferRoleTypeOrganisationStructureMappingDataModel(int offerRoleTypeKey, int organisationStructureKey)
        {
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }
		[JsonConstructor]
        public OfferRoleTypeOrganisationStructureMappingDataModel(int offerRoleTypeOrganisationStructureMappingKey, int offerRoleTypeKey, int organisationStructureKey)
        {
            this.OfferRoleTypeOrganisationStructureMappingKey = offerRoleTypeOrganisationStructureMappingKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }		

        public int OfferRoleTypeOrganisationStructureMappingKey { get; set; }

        public int OfferRoleTypeKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferRoleTypeOrganisationStructureMappingKey =  key;
        }
    }
}