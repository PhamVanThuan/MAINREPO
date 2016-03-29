using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditFinancialServiceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditFinancialServiceDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int financialServiceKey, int accountKey, int? bankAccountKey, double payment, int financialServiceTypeKey, int? debitOrderDay, int? tradeKey, int? categoryKey, int? accountStatusKey, DateTime? nextResetDate, int? parentFinancialServiceKey, DateTime? openDate, DateTime? closeDate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceKey = financialServiceKey;
            this.AccountKey = accountKey;
            this.BankAccountKey = bankAccountKey;
            this.Payment = payment;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.DebitOrderDay = debitOrderDay;
            this.TradeKey = tradeKey;
            this.CategoryKey = categoryKey;
            this.AccountStatusKey = accountStatusKey;
            this.NextResetDate = nextResetDate;
            this.ParentFinancialServiceKey = parentFinancialServiceKey;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
		
        }
		[JsonConstructor]
        public AuditFinancialServiceDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int financialServiceKey, int accountKey, int? bankAccountKey, double payment, int financialServiceTypeKey, int? debitOrderDay, int? tradeKey, int? categoryKey, int? accountStatusKey, DateTime? nextResetDate, int? parentFinancialServiceKey, DateTime? openDate, DateTime? closeDate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceKey = financialServiceKey;
            this.AccountKey = accountKey;
            this.BankAccountKey = bankAccountKey;
            this.Payment = payment;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.DebitOrderDay = debitOrderDay;
            this.TradeKey = tradeKey;
            this.CategoryKey = categoryKey;
            this.AccountStatusKey = accountStatusKey;
            this.NextResetDate = nextResetDate;
            this.ParentFinancialServiceKey = parentFinancialServiceKey;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int FinancialServiceKey { get; set; }

        public int AccountKey { get; set; }

        public int? BankAccountKey { get; set; }

        public double Payment { get; set; }

        public int FinancialServiceTypeKey { get; set; }

        public int? DebitOrderDay { get; set; }

        public int? TradeKey { get; set; }

        public int? CategoryKey { get; set; }

        public int? AccountStatusKey { get; set; }

        public DateTime? NextResetDate { get; set; }

        public int? ParentFinancialServiceKey { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}