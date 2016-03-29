using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditTradeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditTradeDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int tradeKey, string tradeType, string company, DateTime tradeDate, DateTime startDate, DateTime endDate, int resetConfigurationKey, double strikeRate, double tradeBalance, double capBalance, double? premium, int? capTypeKey, int? trancheTypeKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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
		[JsonConstructor]
        public AuditTradeDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int tradeKey, string tradeType, string company, DateTime tradeDate, DateTime startDate, DateTime endDate, int resetConfigurationKey, double strikeRate, double tradeBalance, double capBalance, double? premium, int? capTypeKey, int? trancheTypeKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

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

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}