using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityMarketingOptionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityMarketingOptionDataModel(int legalEntityKey, int marketingOptionKey, DateTime? changeDate, string userID)
        {
            this.LegalEntityKey = legalEntityKey;
            this.MarketingOptionKey = marketingOptionKey;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public LegalEntityMarketingOptionDataModel(int legalEntityMarketingOptionKey, int legalEntityKey, int marketingOptionKey, DateTime? changeDate, string userID)
        {
            this.LegalEntityMarketingOptionKey = legalEntityMarketingOptionKey;
            this.LegalEntityKey = legalEntityKey;
            this.MarketingOptionKey = marketingOptionKey;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }		

        public int LegalEntityMarketingOptionKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int MarketingOptionKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityMarketingOptionKey =  key;
        }
    }
}