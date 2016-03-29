using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureAuctionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ForeclosureAuctionDataModel(int foreclosureInformationKey, int generalStatusKey, int? foreclosureAuctionOutcomeKey, int accountKey)
        {
            this.ForeclosureInformationKey = foreclosureInformationKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ForeclosureAuctionOutcomeKey = foreclosureAuctionOutcomeKey;
            this.AccountKey = accountKey;
		
        }
		[JsonConstructor]
        public ForeclosureAuctionDataModel(int foreclosureAuctionKey, int foreclosureInformationKey, int generalStatusKey, int? foreclosureAuctionOutcomeKey, int accountKey)
        {
            this.ForeclosureAuctionKey = foreclosureAuctionKey;
            this.ForeclosureInformationKey = foreclosureInformationKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ForeclosureAuctionOutcomeKey = foreclosureAuctionOutcomeKey;
            this.AccountKey = accountKey;
		
        }		

        public int ForeclosureAuctionKey { get; set; }

        public int ForeclosureInformationKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int? ForeclosureAuctionOutcomeKey { get; set; }

        public int AccountKey { get; set; }

        public void SetKey(int key)
        {
            this.ForeclosureAuctionKey =  key;
        }
    }
}