using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferAttributeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferAttributeDataModel(int offerKey, int offerAttributeTypeKey)
        {
            this.OfferKey = offerKey;
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
		
        }
		[JsonConstructor]
        public OfferAttributeDataModel(int offerAttributeKey, int offerKey, int offerAttributeTypeKey)
        {
            this.OfferAttributeKey = offerAttributeKey;
            this.OfferKey = offerKey;
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
		
        }		

        public int OfferAttributeKey { get; set; }

        public int OfferKey { get; set; }

        public int OfferAttributeTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferAttributeKey =  key;
        }
    }
}