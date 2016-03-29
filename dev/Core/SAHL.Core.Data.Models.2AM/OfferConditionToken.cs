using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferConditionTokenDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferConditionTokenDataModel(int tokenKey, int offerConditionKey, int? translatableItemKey, string tokenValue)
        {
            this.TokenKey = tokenKey;
            this.OfferConditionKey = offerConditionKey;
            this.TranslatableItemKey = translatableItemKey;
            this.TokenValue = tokenValue;
		
        }
		[JsonConstructor]
        public OfferConditionTokenDataModel(int offerConditionTokenKey, int tokenKey, int offerConditionKey, int? translatableItemKey, string tokenValue)
        {
            this.OfferConditionTokenKey = offerConditionTokenKey;
            this.TokenKey = tokenKey;
            this.OfferConditionKey = offerConditionKey;
            this.TranslatableItemKey = translatableItemKey;
            this.TokenValue = tokenValue;
		
        }		

        public int OfferConditionTokenKey { get; set; }

        public int TokenKey { get; set; }

        public int OfferConditionKey { get; set; }

        public int? TranslatableItemKey { get; set; }

        public string TokenValue { get; set; }

        public void SetKey(int key)
        {
            this.OfferConditionTokenKey =  key;
        }
    }
}