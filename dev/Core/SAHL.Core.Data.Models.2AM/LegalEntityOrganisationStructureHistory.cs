using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityOrganisationStructureHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityOrganisationStructureHistoryDataModel(int legalEntityOrganisationStructureKey, int legalEntityKey, int organisationStructureKey, DateTime? changeDate, string action)
        {
            this.LegalEntityOrganisationStructureKey = legalEntityOrganisationStructureKey;
            this.LegalEntityKey = legalEntityKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.ChangeDate = changeDate;
            this.Action = action;
		
        }
		[JsonConstructor]
        public LegalEntityOrganisationStructureHistoryDataModel(int legalEntityOrganisationStructureHistoryKey, int legalEntityOrganisationStructureKey, int legalEntityKey, int organisationStructureKey, DateTime? changeDate, string action)
        {
            this.LegalEntityOrganisationStructureHistoryKey = legalEntityOrganisationStructureHistoryKey;
            this.LegalEntityOrganisationStructureKey = legalEntityOrganisationStructureKey;
            this.LegalEntityKey = legalEntityKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.ChangeDate = changeDate;
            this.Action = action;
		
        }		

        public int LegalEntityOrganisationStructureHistoryKey { get; set; }

        public int LegalEntityOrganisationStructureKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string Action { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityOrganisationStructureHistoryKey =  key;
        }
    }
}