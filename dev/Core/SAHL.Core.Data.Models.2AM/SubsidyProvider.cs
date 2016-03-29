using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SubsidyProviderDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SubsidyProviderDataModel(int subsidyProviderTypeKey, string persalOrganisationCode, string contactPerson, string userID, DateTime? changeDate, int legalEntityKey, bool gEPFAffiliate)
        {
            this.SubsidyProviderTypeKey = subsidyProviderTypeKey;
            this.PersalOrganisationCode = persalOrganisationCode;
            this.ContactPerson = contactPerson;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.LegalEntityKey = legalEntityKey;
            this.GEPFAffiliate = gEPFAffiliate;
		
        }
		[JsonConstructor]
        public SubsidyProviderDataModel(int subsidyProviderKey, int subsidyProviderTypeKey, string persalOrganisationCode, string contactPerson, string userID, DateTime? changeDate, int legalEntityKey, bool gEPFAffiliate)
        {
            this.SubsidyProviderKey = subsidyProviderKey;
            this.SubsidyProviderTypeKey = subsidyProviderTypeKey;
            this.PersalOrganisationCode = persalOrganisationCode;
            this.ContactPerson = contactPerson;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.LegalEntityKey = legalEntityKey;
            this.GEPFAffiliate = gEPFAffiliate;
		
        }		

        public int SubsidyProviderKey { get; set; }

        public int SubsidyProviderTypeKey { get; set; }

        public string PersalOrganisationCode { get; set; }

        public string ContactPerson { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int LegalEntityKey { get; set; }

        public bool GEPFAffiliate { get; set; }

        public void SetKey(int key)
        {
            this.SubsidyProviderKey =  key;
        }
    }
}