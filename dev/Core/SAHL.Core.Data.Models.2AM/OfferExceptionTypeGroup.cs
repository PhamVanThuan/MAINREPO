using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferExceptionTypeGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferExceptionTypeGroupDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public OfferExceptionTypeGroupDataModel(int offerExceptionTypeGroupKey, string description)
        {
            this.OfferExceptionTypeGroupKey = offerExceptionTypeGroupKey;
            this.Description = description;
		
        }		

        public int OfferExceptionTypeGroupKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.OfferExceptionTypeGroupKey =  key;
        }
    }
}