using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferCampaignDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferCampaignDataModel(string description, DateTime startDate)
        {
            this.Description = description;
            this.StartDate = startDate;
		
        }
		[JsonConstructor]
        public OfferCampaignDataModel(int offerCampaignKey, string description, DateTime startDate)
        {
            this.OfferCampaignKey = offerCampaignKey;
            this.Description = description;
            this.StartDate = startDate;
		
        }		

        public int OfferCampaignKey { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public void SetKey(int key)
        {
            this.OfferCampaignKey =  key;
        }
    }
}