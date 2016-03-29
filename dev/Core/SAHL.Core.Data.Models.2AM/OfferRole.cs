using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferRoleDataModel(int legalEntityKey, int offerKey, int offerRoleTypeKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.LegalEntityKey = legalEntityKey;
            this.OfferKey = offerKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }
		[JsonConstructor]
        public OfferRoleDataModel(int offerRoleKey, int legalEntityKey, int offerKey, int offerRoleTypeKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.OfferRoleKey = offerRoleKey;
            this.LegalEntityKey = legalEntityKey;
            this.OfferKey = offerKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }		

        public int OfferRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int OfferKey { get; set; }

        public int OfferRoleTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.OfferRoleKey =  key;
        }
    }
}