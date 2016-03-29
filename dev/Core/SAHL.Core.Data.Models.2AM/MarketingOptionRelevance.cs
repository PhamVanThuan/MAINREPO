using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MarketingOptionRelevanceDataModel :  IDataModel
    {
        public MarketingOptionRelevanceDataModel(int marketingOptionRelevanceKey, string description)
        {
            this.MarketingOptionRelevanceKey = marketingOptionRelevanceKey;
            this.Description = description;
		
        }		

        public int MarketingOptionRelevanceKey { get; set; }

        public string Description { get; set; }
    }
}