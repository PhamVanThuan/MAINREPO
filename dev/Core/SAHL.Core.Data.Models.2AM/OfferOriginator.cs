using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferOriginatorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferOriginatorDataModel(int legalEntityKey, string contact, int generalStatusKey, int originationSourceKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.Contact = contact;
            this.GeneralStatusKey = generalStatusKey;
            this.OriginationSourceKey = originationSourceKey;
		
        }
		[JsonConstructor]
        public OfferOriginatorDataModel(int offerOriginatorKey, int legalEntityKey, string contact, int generalStatusKey, int originationSourceKey)
        {
            this.OfferOriginatorKey = offerOriginatorKey;
            this.LegalEntityKey = legalEntityKey;
            this.Contact = contact;
            this.GeneralStatusKey = generalStatusKey;
            this.OriginationSourceKey = originationSourceKey;
		
        }		

        public int OfferOriginatorKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string Contact { get; set; }

        public int GeneralStatusKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferOriginatorKey =  key;
        }
    }
}