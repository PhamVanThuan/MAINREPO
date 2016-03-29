using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferMarketingSurveyTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferMarketingSurveyTypeDataModel(string description, int offerMarketingSurveyTypeGroupKey)
        {
            this.Description = description;
            this.OfferMarketingSurveyTypeGroupKey = offerMarketingSurveyTypeGroupKey;
		
        }
		[JsonConstructor]
        public OfferMarketingSurveyTypeDataModel(int offerMarketingSurveyTypeKey, string description, int offerMarketingSurveyTypeGroupKey)
        {
            this.OfferMarketingSurveyTypeKey = offerMarketingSurveyTypeKey;
            this.Description = description;
            this.OfferMarketingSurveyTypeGroupKey = offerMarketingSurveyTypeGroupKey;
		
        }		

        public int OfferMarketingSurveyTypeKey { get; set; }

        public string Description { get; set; }

        public int OfferMarketingSurveyTypeGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferMarketingSurveyTypeKey =  key;
        }
    }
}