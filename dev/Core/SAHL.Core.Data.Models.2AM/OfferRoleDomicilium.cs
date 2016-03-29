using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleDomiciliumDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferRoleDomiciliumDataModel(int legalEntityDomiciliumKey, int offerRoleKey, DateTime changeDate, int aDUserKey)
        {
            this.LegalEntityDomiciliumKey = legalEntityDomiciliumKey;
            this.OfferRoleKey = offerRoleKey;
            this.ChangeDate = changeDate;
            this.ADUserKey = aDUserKey;
		
        }
		[JsonConstructor]
        public OfferRoleDomiciliumDataModel(int offerRoleDomiciliumKey, int legalEntityDomiciliumKey, int offerRoleKey, DateTime changeDate, int aDUserKey)
        {
            this.OfferRoleDomiciliumKey = offerRoleDomiciliumKey;
            this.LegalEntityDomiciliumKey = legalEntityDomiciliumKey;
            this.OfferRoleKey = offerRoleKey;
            this.ChangeDate = changeDate;
            this.ADUserKey = aDUserKey;
		
        }		

        public int OfferRoleDomiciliumKey { get; set; }

        public int LegalEntityDomiciliumKey { get; set; }

        public int OfferRoleKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public int ADUserKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferRoleDomiciliumKey =  key;
        }
    }
}