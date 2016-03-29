using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityMarketingOptionHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityMarketingOptionHistoryDataModel(int legalEntityMarketingOptionKey, int legalEntityKey, int marketingOptionKey, string changeAction, DateTime? changeDate, string userID)
        {
            this.LegalEntityMarketingOptionKey = legalEntityMarketingOptionKey;
            this.LegalEntityKey = legalEntityKey;
            this.MarketingOptionKey = marketingOptionKey;
            this.ChangeAction = changeAction;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public LegalEntityMarketingOptionHistoryDataModel(int legalEntityMarketingOptionHistoryKey, int legalEntityMarketingOptionKey, int legalEntityKey, int marketingOptionKey, string changeAction, DateTime? changeDate, string userID)
        {
            this.LegalEntityMarketingOptionHistoryKey = legalEntityMarketingOptionHistoryKey;
            this.LegalEntityMarketingOptionKey = legalEntityMarketingOptionKey;
            this.LegalEntityKey = legalEntityKey;
            this.MarketingOptionKey = marketingOptionKey;
            this.ChangeAction = changeAction;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }		

        public int LegalEntityMarketingOptionHistoryKey { get; set; }

        public int LegalEntityMarketingOptionKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int MarketingOptionKey { get; set; }

        public string ChangeAction { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityMarketingOptionHistoryKey =  key;
        }
    }
}