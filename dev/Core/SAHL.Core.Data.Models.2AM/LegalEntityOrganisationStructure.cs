using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityOrganisationStructureDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityOrganisationStructureDataModel(int legalEntityKey, int organisationStructureKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }
		[JsonConstructor]
        public LegalEntityOrganisationStructureDataModel(int legalEntityOrganisationStructureKey, int legalEntityKey, int organisationStructureKey)
        {
            this.LegalEntityOrganisationStructureKey = legalEntityOrganisationStructureKey;
            this.LegalEntityKey = legalEntityKey;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }		

        public int LegalEntityOrganisationStructureKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityOrganisationStructureKey =  key;
        }
    }
}