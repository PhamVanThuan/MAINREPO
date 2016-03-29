using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureAuctionOutcomeDataModel :  IDataModel
    {
        public ForeclosureAuctionOutcomeDataModel(int foreclosureAuctionOutcomeKey, string description)
        {
            this.ForeclosureAuctionOutcomeKey = foreclosureAuctionOutcomeKey;
            this.Description = description;
		
        }		

        public int ForeclosureAuctionOutcomeKey { get; set; }

        public string Description { get; set; }
    }
}