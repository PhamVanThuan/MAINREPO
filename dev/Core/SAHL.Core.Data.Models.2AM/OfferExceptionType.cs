using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferExceptionTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferExceptionTypeDataModel(string description, int offerExceptionTypeGroupKey)
        {
            this.Description = description;
            this.OfferExceptionTypeGroupKey = offerExceptionTypeGroupKey;
		
        }
		[JsonConstructor]
        public OfferExceptionTypeDataModel(int offerExceptionTypeKey, string description, int offerExceptionTypeGroupKey)
        {
            this.OfferExceptionTypeKey = offerExceptionTypeKey;
            this.Description = description;
            this.OfferExceptionTypeGroupKey = offerExceptionTypeGroupKey;
		
        }		

        public int OfferExceptionTypeKey { get; set; }

        public string Description { get; set; }

        public int OfferExceptionTypeGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferExceptionTypeKey =  key;
        }
    }
}