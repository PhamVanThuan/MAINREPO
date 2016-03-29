using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditMortgageLoanInfoDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditMortgageLoanInfoDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime? auditDate, string auditAddUpdateDelete, int mortgageLoanInfoKey, int financialServiceKey, DateTime? electionDate, DateTime? convertedDate, double? accumulatedLoyaltyBenefit, DateTime? nextPaymentDate, double? discountRate, double? pPThresholdYr1, double? pPThresholdYr2, double? pPThresholdYr3, double? pPThresholdYr4, double? pPThresholdYr5, double? mTDLoyaltyBenefit, double? pPAllowed, int? generalStatusKey, string exclusion, DateTime? exclusionEndDate, string exclusionReason, string overPaymentAmount)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MortgageLoanInfoKey = mortgageLoanInfoKey;
            this.FinancialServiceKey = financialServiceKey;
            this.ElectionDate = electionDate;
            this.ConvertedDate = convertedDate;
            this.AccumulatedLoyaltyBenefit = accumulatedLoyaltyBenefit;
            this.NextPaymentDate = nextPaymentDate;
            this.DiscountRate = discountRate;
            this.PPThresholdYr1 = pPThresholdYr1;
            this.PPThresholdYr2 = pPThresholdYr2;
            this.PPThresholdYr3 = pPThresholdYr3;
            this.PPThresholdYr4 = pPThresholdYr4;
            this.PPThresholdYr5 = pPThresholdYr5;
            this.MTDLoyaltyBenefit = mTDLoyaltyBenefit;
            this.PPAllowed = pPAllowed;
            this.GeneralStatusKey = generalStatusKey;
            this.Exclusion = exclusion;
            this.ExclusionEndDate = exclusionEndDate;
            this.ExclusionReason = exclusionReason;
            this.OverPaymentAmount = overPaymentAmount;
		
        }
		[JsonConstructor]
        public AuditMortgageLoanInfoDataModel(int auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime? auditDate, string auditAddUpdateDelete, int mortgageLoanInfoKey, int financialServiceKey, DateTime? electionDate, DateTime? convertedDate, double? accumulatedLoyaltyBenefit, DateTime? nextPaymentDate, double? discountRate, double? pPThresholdYr1, double? pPThresholdYr2, double? pPThresholdYr3, double? pPThresholdYr4, double? pPThresholdYr5, double? mTDLoyaltyBenefit, double? pPAllowed, int? generalStatusKey, string exclusion, DateTime? exclusionEndDate, string exclusionReason, string overPaymentAmount)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MortgageLoanInfoKey = mortgageLoanInfoKey;
            this.FinancialServiceKey = financialServiceKey;
            this.ElectionDate = electionDate;
            this.ConvertedDate = convertedDate;
            this.AccumulatedLoyaltyBenefit = accumulatedLoyaltyBenefit;
            this.NextPaymentDate = nextPaymentDate;
            this.DiscountRate = discountRate;
            this.PPThresholdYr1 = pPThresholdYr1;
            this.PPThresholdYr2 = pPThresholdYr2;
            this.PPThresholdYr3 = pPThresholdYr3;
            this.PPThresholdYr4 = pPThresholdYr4;
            this.PPThresholdYr5 = pPThresholdYr5;
            this.MTDLoyaltyBenefit = mTDLoyaltyBenefit;
            this.PPAllowed = pPAllowed;
            this.GeneralStatusKey = generalStatusKey;
            this.Exclusion = exclusion;
            this.ExclusionEndDate = exclusionEndDate;
            this.ExclusionReason = exclusionReason;
            this.OverPaymentAmount = overPaymentAmount;
		
        }		

        public int AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime? AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int MortgageLoanInfoKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public DateTime? ElectionDate { get; set; }

        public DateTime? ConvertedDate { get; set; }

        public double? AccumulatedLoyaltyBenefit { get; set; }

        public DateTime? NextPaymentDate { get; set; }

        public double? DiscountRate { get; set; }

        public double? PPThresholdYr1 { get; set; }

        public double? PPThresholdYr2 { get; set; }

        public double? PPThresholdYr3 { get; set; }

        public double? PPThresholdYr4 { get; set; }

        public double? PPThresholdYr5 { get; set; }

        public double? MTDLoyaltyBenefit { get; set; }

        public double? PPAllowed { get; set; }

        public int? GeneralStatusKey { get; set; }

        public string Exclusion { get; set; }

        public DateTime? ExclusionEndDate { get; set; }

        public string ExclusionReason { get; set; }

        public string OverPaymentAmount { get; set; }

        public void SetKey(int key)
        {
            this.AuditNumber =  key;
        }
    }
}