using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationVariableLoanDataModel :  IDataModel
    {
        public OfferInformationVariableLoanDataModel(int offerInformationKey, int? categoryKey, int? term, double? existingLoan, double? cashDeposit, double? propertyValuation, double? householdIncome, double? feesTotal, double? interimInterest, double? monthlyInstalment, double? lifePremium, double? hOCPremium, double? minLoanRequired, double? minBondRequired, double? preApprovedAmount, double? minCashAllowed, double? maxCashAllowed, double? loanAmountNoFees, double? requestedCashAmount, double? loanAgreementAmount, double? bondToRegister, double? lTV, double? pTI, double? marketRate, int? sPVKey, int? employmentTypeKey, int? rateConfigurationKey, int? creditMatrixKey, int? creditCriteriaKey, decimal? appliedInitiationFeeDiscount)
        {
            this.OfferInformationKey = offerInformationKey;
            this.CategoryKey = categoryKey;
            this.Term = term;
            this.ExistingLoan = existingLoan;
            this.CashDeposit = cashDeposit;
            this.PropertyValuation = propertyValuation;
            this.HouseholdIncome = householdIncome;
            this.FeesTotal = feesTotal;
            this.InterimInterest = interimInterest;
            this.MonthlyInstalment = monthlyInstalment;
            this.LifePremium = lifePremium;
            this.HOCPremium = hOCPremium;
            this.MinLoanRequired = minLoanRequired;
            this.MinBondRequired = minBondRequired;
            this.PreApprovedAmount = preApprovedAmount;
            this.MinCashAllowed = minCashAllowed;
            this.MaxCashAllowed = maxCashAllowed;
            this.LoanAmountNoFees = loanAmountNoFees;
            this.RequestedCashAmount = requestedCashAmount;
            this.LoanAgreementAmount = loanAgreementAmount;
            this.BondToRegister = bondToRegister;
            this.LTV = lTV;
            this.PTI = pTI;
            this.MarketRate = marketRate;
            this.SPVKey = sPVKey;
            this.EmploymentTypeKey = employmentTypeKey;
            this.RateConfigurationKey = rateConfigurationKey;
            this.CreditMatrixKey = creditMatrixKey;
            this.CreditCriteriaKey = creditCriteriaKey;
            this.AppliedInitiationFeeDiscount = appliedInitiationFeeDiscount;
		
        }		

        public int OfferInformationKey { get; set; }

        public int? CategoryKey { get; set; }

        public int? Term { get; set; }

        public double? ExistingLoan { get; set; }

        public double? CashDeposit { get; set; }

        public double? PropertyValuation { get; set; }

        public double? HouseholdIncome { get; set; }

        public double? FeesTotal { get; set; }

        public double? InterimInterest { get; set; }

        public double? MonthlyInstalment { get; set; }

        public double? LifePremium { get; set; }

        public double? HOCPremium { get; set; }

        public double? MinLoanRequired { get; set; }

        public double? MinBondRequired { get; set; }

        public double? PreApprovedAmount { get; set; }

        public double? MinCashAllowed { get; set; }

        public double? MaxCashAllowed { get; set; }

        public double? LoanAmountNoFees { get; set; }

        public double? RequestedCashAmount { get; set; }

        public double? LoanAgreementAmount { get; set; }

        public double? BondToRegister { get; set; }

        public double? LTV { get; set; }

        public double? PTI { get; set; }

        public double? MarketRate { get; set; }

        public int? SPVKey { get; set; }

        public int? EmploymentTypeKey { get; set; }

        public int? RateConfigurationKey { get; set; }

        public int? CreditMatrixKey { get; set; }

        public int? CreditCriteriaKey { get; set; }

        public decimal? AppliedInitiationFeeDiscount { get; set; }
    }
}