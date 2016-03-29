using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductOfferAttributeTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductOfferAttributeTypeDataModel(int originationSourceProductKey, int offerAttributeTypeKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductOfferAttributeTypeDataModel(int originationSourceProductOfferAttributeTypeKey, int originationSourceProductKey, int offerAttributeTypeKey)
        {
            this.OriginationSourceProductOfferAttributeTypeKey = originationSourceProductOfferAttributeTypeKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
		
        }		

        public int OriginationSourceProductOfferAttributeTypeKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public int OfferAttributeTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceProductOfferAttributeTypeKey =  key;
        }
    }
}