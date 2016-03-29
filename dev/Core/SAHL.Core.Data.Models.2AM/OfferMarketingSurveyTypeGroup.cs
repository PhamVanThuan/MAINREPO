using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferMarketingSurveyTypeGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferMarketingSurveyTypeGroupDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public OfferMarketingSurveyTypeGroupDataModel(int offerMarketingSurveyTypeGroupKey, string description)
        {
            this.OfferMarketingSurveyTypeGroupKey = offerMarketingSurveyTypeGroupKey;
            this.Description = description;
		
        }		

        public int OfferMarketingSurveyTypeGroupKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.OfferMarketingSurveyTypeGroupKey =  key;
        }
    }
}