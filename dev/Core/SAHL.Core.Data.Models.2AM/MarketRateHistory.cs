using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MarketRateHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MarketRateHistoryDataModel(DateTime? changeDate, double rateBefore, double rateAfter, int marketRateKey, string changedBy, string changedByHost, string changedByApp)
        {
            this.ChangeDate = changeDate;
            this.RateBefore = rateBefore;
            this.RateAfter = rateAfter;
            this.MarketRateKey = marketRateKey;
            this.ChangedBy = changedBy;
            this.ChangedByHost = changedByHost;
            this.ChangedByApp = changedByApp;
		
        }
		[JsonConstructor]
        public MarketRateHistoryDataModel(int marketRateHistoryKey, DateTime? changeDate, double rateBefore, double rateAfter, int marketRateKey, string changedBy, string changedByHost, string changedByApp)
        {
            this.MarketRateHistoryKey = marketRateHistoryKey;
            this.ChangeDate = changeDate;
            this.RateBefore = rateBefore;
            this.RateAfter = rateAfter;
            this.MarketRateKey = marketRateKey;
            this.ChangedBy = changedBy;
            this.ChangedByHost = changedByHost;
            this.ChangedByApp = changedByApp;
		
        }		

        public int MarketRateHistoryKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public double RateBefore { get; set; }

        public double RateAfter { get; set; }

        public int MarketRateKey { get; set; }

        public string ChangedBy { get; set; }

        public string ChangedByHost { get; set; }

        public string ChangedByApp { get; set; }

        public void SetKey(int key)
        {
            this.MarketRateHistoryKey =  key;
        }
    }
}