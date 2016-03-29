using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TradeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TradeDataModel(string tradeType, string company, DateTime tradeDate, DateTime startDate, DateTime endDate, int resetConfigurationKey, double strikeRate, double tradeBalance, double capBalance, double? premium, int? capTypeKey, int? trancheTypeKey)
        {
            this.TradeType = tradeType;
            this.Company = company;
            this.TradeDate = tradeDate;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.StrikeRate = strikeRate;
            this.TradeBalance = tradeBalance;
            this.CapBalance = capBalance;
            this.Premium = premium;
            this.CapTypeKey = capTypeKey;
            this.TrancheTypeKey = trancheTypeKey;
		
        }
		[JsonConstructor]
        public TradeDataModel(int tradeKey, string tradeType, string company, DateTime tradeDate, DateTime startDate, DateTime endDate, int resetConfigurationKey, double strikeRate, double tradeBalance, double capBalance, double? premium, int? capTypeKey, int? trancheTypeKey)
        {
            this.TradeKey = tradeKey;
            this.TradeType = tradeType;
            this.Company = company;
            this.TradeDate = tradeDate;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.StrikeRate = strikeRate;
            this.TradeBalance = tradeBalance;
            this.CapBalance = capBalance;
            this.Premium = premium;
            this.CapTypeKey = capTypeKey;
            this.TrancheTypeKey = trancheTypeKey;
		
        }		

        public int TradeKey { get; set; }

        public string TradeType { get; set; }

        public string Company { get; set; }

        public DateTime TradeDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ResetConfigurationKey { get; set; }

        public double StrikeRate { get; set; }

        public double TradeBalance { get; set; }

        public double CapBalance { get; set; }

        public double? Premium { get; set; }

        public int? CapTypeKey { get; set; }

        public int? TrancheTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.TradeKey =  key;
        }
    }
}