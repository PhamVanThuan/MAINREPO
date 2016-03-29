using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferMarketingSurveyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferMarketingSurveyDataModel(int offerKey, int offerMarketingSurveyTypeKey)
        {
            this.OfferKey = offerKey;
            this.OfferMarketingSurveyTypeKey = offerMarketingSurveyTypeKey;
		
        }
		[JsonConstructor]
        public OfferMarketingSurveyDataModel(int offerMarketingSurveyKey, int offerKey, int offerMarketingSurveyTypeKey)
        {
            this.OfferMarketingSurveyKey = offerMarketingSurveyKey;
            this.OfferKey = offerKey;
            this.OfferMarketingSurveyTypeKey = offerMarketingSurveyTypeKey;
		
        }		

        public int OfferMarketingSurveyKey { get; set; }

        public int OfferKey { get; set; }

        public int OfferMarketingSurveyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferMarketingSurveyKey =  key;
        }
    }
}