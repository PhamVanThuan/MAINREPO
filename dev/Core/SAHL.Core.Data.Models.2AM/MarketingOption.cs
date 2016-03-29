using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MarketingOptionDataModel :  IDataModel
    {
        public MarketingOptionDataModel(int marketingOptionKey, string description, int generalStatusKey)
        {
            this.MarketingOptionKey = marketingOptionKey;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int MarketingOptionKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}