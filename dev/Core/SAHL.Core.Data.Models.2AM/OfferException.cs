using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferExceptionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferExceptionDataModel(int offerKey, int offerExceptionTypeKey, int overRidden)
        {
            this.OfferKey = offerKey;
            this.OfferExceptionTypeKey = offerExceptionTypeKey;
            this.OverRidden = overRidden;
		
        }
		[JsonConstructor]
        public OfferExceptionDataModel(int offerExceptionKey, int offerKey, int offerExceptionTypeKey, int overRidden)
        {
            this.OfferExceptionKey = offerExceptionKey;
            this.OfferKey = offerKey;
            this.OfferExceptionTypeKey = offerExceptionTypeKey;
            this.OverRidden = overRidden;
		
        }		

        public int OfferExceptionKey { get; set; }

        public int OfferKey { get; set; }

        public int OfferExceptionTypeKey { get; set; }

        public int OverRidden { get; set; }

        public void SetKey(int key)
        {
            this.OfferExceptionKey =  key;
        }
    }
}