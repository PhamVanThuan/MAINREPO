using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FinancialServiceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FinancialServiceDataModel(int accountKey, double payment, int financialServiceTypeKey, int? tradeKey, int? categoryKey, int accountStatusKey, DateTime? nextResetDate, int? parentFinancialServiceKey, DateTime? openDate, DateTime? closeDate)
        {
            this.AccountKey = accountKey;
            this.Payment = payment;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.TradeKey = tradeKey;
            this.CategoryKey = categoryKey;
            this.AccountStatusKey = accountStatusKey;
            this.NextResetDate = nextResetDate;
            this.ParentFinancialServiceKey = parentFinancialServiceKey;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
		
        }
		[JsonConstructor]
        public FinancialServiceDataModel(int financialServiceKey, int accountKey, double payment, int financialServiceTypeKey, int? tradeKey, int? categoryKey, int accountStatusKey, DateTime? nextResetDate, int? parentFinancialServiceKey, DateTime? openDate, DateTime? closeDate)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.AccountKey = accountKey;
            this.Payment = payment;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.TradeKey = tradeKey;
            this.CategoryKey = categoryKey;
            this.AccountStatusKey = accountStatusKey;
            this.NextResetDate = nextResetDate;
            this.ParentFinancialServiceKey = parentFinancialServiceKey;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
		
        }		

        public int FinancialServiceKey { get; set; }

        public int AccountKey { get; set; }

        public double Payment { get; set; }

        public int FinancialServiceTypeKey { get; set; }

        public int? TradeKey { get; set; }

        public int? CategoryKey { get; set; }

        public int AccountStatusKey { get; set; }

        public DateTime? NextResetDate { get; set; }

        public int? ParentFinancialServiceKey { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public void SetKey(int key)
        {
            this.FinancialServiceKey =  key;
        }
    }
}