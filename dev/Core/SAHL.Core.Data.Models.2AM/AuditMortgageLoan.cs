using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditMortgageLoanDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditMortgageLoanDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int financialServiceKey, short initialInstallments, double initialBalance, short remainingInstallments, double currentBalance, double arrearBalance, DateTime? closeDate, DateTime openDate, double interestRate, int sPVKey, int rateConfigurationKey, int resetConfigurationKey, double? accruedInterestMTD, int? mortgageLoanPurposeKey, int? creditMatrixKey, double? preApproved, double? discount, double? baseRate, string userID, DateTime? changeDate, double? activeMarketRate, double? accumulatedCoPayment, double? mTDCoPayment)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceKey = financialServiceKey;
            this.InitialInstallments = initialInstallments;
            this.InitialBalance = initialBalance;
            this.RemainingInstallments = remainingInstallments;
            this.CurrentBalance = currentBalance;
            this.ArrearBalance = arrearBalance;
            this.CloseDate = closeDate;
            this.OpenDate = openDate;
            this.InterestRate = interestRate;
            this.SPVKey = sPVKey;
            this.RateConfigurationKey = rateConfigurationKey;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.AccruedInterestMTD = accruedInterestMTD;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.CreditMatrixKey = creditMatrixKey;
            this.PreApproved = preApproved;
            this.Discount = discount;
            this.BaseRate = baseRate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.ActiveMarketRate = activeMarketRate;
            this.AccumulatedCoPayment = accumulatedCoPayment;
            this.MTDCoPayment = mTDCoPayment;
		
        }
		[JsonConstructor]
        public AuditMortgageLoanDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int financialServiceKey, short initialInstallments, double initialBalance, short remainingInstallments, double currentBalance, double arrearBalance, DateTime? closeDate, DateTime openDate, double interestRate, int sPVKey, int rateConfigurationKey, int resetConfigurationKey, double? accruedInterestMTD, int? mortgageLoanPurposeKey, int? creditMatrixKey, double? preApproved, double? discount, double? baseRate, string userID, DateTime? changeDate, double? activeMarketRate, double? accumulatedCoPayment, double? mTDCoPayment)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceKey = financialServiceKey;
            this.InitialInstallments = initialInstallments;
            this.InitialBalance = initialBalance;
            this.RemainingInstallments = remainingInstallments;
            this.CurrentBalance = currentBalance;
            this.ArrearBalance = arrearBalance;
            this.CloseDate = closeDate;
            this.OpenDate = openDate;
            this.InterestRate = interestRate;
            this.SPVKey = sPVKey;
            this.RateConfigurationKey = rateConfigurationKey;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.AccruedInterestMTD = accruedInterestMTD;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.CreditMatrixKey = creditMatrixKey;
            this.PreApproved = preApproved;
            this.Discount = discount;
            this.BaseRate = baseRate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.ActiveMarketRate = activeMarketRate;
            this.AccumulatedCoPayment = accumulatedCoPayment;
            this.MTDCoPayment = mTDCoPayment;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int FinancialServiceKey { get; set; }

        public short InitialInstallments { get; set; }

        public double InitialBalance { get; set; }

        public short RemainingInstallments { get; set; }

        public double CurrentBalance { get; set; }

        public double ArrearBalance { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime OpenDate { get; set; }

        public double InterestRate { get; set; }

        public int SPVKey { get; set; }

        public int RateConfigurationKey { get; set; }

        public int ResetConfigurationKey { get; set; }

        public double? AccruedInterestMTD { get; set; }

        public int? MortgageLoanPurposeKey { get; set; }

        public int? CreditMatrixKey { get; set; }

        public double? PreApproved { get; set; }

        public double? Discount { get; set; }

        public double? BaseRate { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public double? ActiveMarketRate { get; set; }

        public double? AccumulatedCoPayment { get; set; }

        public double? MTDCoPayment { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}