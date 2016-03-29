using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferConditionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferConditionDataModel(int offerKey, int conditionKey, int translatableItemKey, int? conditionSetKey)
        {
            this.OfferKey = offerKey;
            this.ConditionKey = conditionKey;
            this.TranslatableItemKey = translatableItemKey;
            this.ConditionSetKey = conditionSetKey;
		
        }
		[JsonConstructor]
        public OfferConditionDataModel(int offerConditionKey, int offerKey, int conditionKey, int translatableItemKey, int? conditionSetKey)
        {
            this.OfferConditionKey = offerConditionKey;
            this.OfferKey = offerKey;
            this.ConditionKey = conditionKey;
            this.TranslatableItemKey = translatableItemKey;
            this.ConditionSetKey = conditionSetKey;
		
        }		

        public int OfferConditionKey { get; set; }

        public int OfferKey { get; set; }

        public int ConditionKey { get; set; }

        public int TranslatableItemKey { get; set; }

        public int? ConditionSetKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferConditionKey =  key;
        }
    }
}