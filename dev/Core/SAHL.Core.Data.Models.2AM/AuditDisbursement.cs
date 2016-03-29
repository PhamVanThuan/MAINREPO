using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditDisbursementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditDisbursementDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int disbursementKey, int accountKey, int? aCBBankCode, string aCBBranchCode, int? aCBTypeNumber, DateTime? preparedDate, DateTime? actionDate, string accountName, string accountNumber, double? amount, int? disbursementStatusKey, int? disbursementTransactionTypeKey, double? capitalAmount, double? guaranteeAmount, double? interestRate, DateTime? interestStartDate, string interestApplied, double? paymentAmount)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.DisbursementKey = disbursementKey;
            this.AccountKey = accountKey;
            this.ACBBankCode = aCBBankCode;
            this.ACBBranchCode = aCBBranchCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.PreparedDate = preparedDate;
            this.ActionDate = actionDate;
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
            this.Amount = amount;
            this.DisbursementStatusKey = disbursementStatusKey;
            this.DisbursementTransactionTypeKey = disbursementTransactionTypeKey;
            this.CapitalAmount = capitalAmount;
            this.GuaranteeAmount = guaranteeAmount;
            this.InterestRate = interestRate;
            this.InterestStartDate = interestStartDate;
            this.InterestApplied = interestApplied;
            this.PaymentAmount = paymentAmount;
		
        }
		[JsonConstructor]
        public AuditDisbursementDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int disbursementKey, int accountKey, int? aCBBankCode, string aCBBranchCode, int? aCBTypeNumber, DateTime? preparedDate, DateTime? actionDate, string accountName, string accountNumber, double? amount, int? disbursementStatusKey, int? disbursementTransactionTypeKey, double? capitalAmount, double? guaranteeAmount, double? interestRate, DateTime? interestStartDate, string interestApplied, double? paymentAmount)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.DisbursementKey = disbursementKey;
            this.AccountKey = accountKey;
            this.ACBBankCode = aCBBankCode;
            this.ACBBranchCode = aCBBranchCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.PreparedDate = preparedDate;
            this.ActionDate = actionDate;
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
            this.Amount = amount;
            this.DisbursementStatusKey = disbursementStatusKey;
            this.DisbursementTransactionTypeKey = disbursementTransactionTypeKey;
            this.CapitalAmount = capitalAmount;
            this.GuaranteeAmount = guaranteeAmount;
            this.InterestRate = interestRate;
            this.InterestStartDate = interestStartDate;
            this.InterestApplied = interestApplied;
            this.PaymentAmount = paymentAmount;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int DisbursementKey { get; set; }

        public int AccountKey { get; set; }

        public int? ACBBankCode { get; set; }

        public string ACBBranchCode { get; set; }

        public int? ACBTypeNumber { get; set; }

        public DateTime? PreparedDate { get; set; }

        public DateTime? ActionDate { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public double? Amount { get; set; }

        public int? DisbursementStatusKey { get; set; }

        public int? DisbursementTransactionTypeKey { get; set; }

        public double? CapitalAmount { get; set; }

        public double? GuaranteeAmount { get; set; }

        public double? InterestRate { get; set; }

        public DateTime? InterestStartDate { get; set; }

        public string InterestApplied { get; set; }

        public double? PaymentAmount { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}