using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportOfferInformationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportOfferInformationDataModel(int offerKey, int? offerTerm, double? cashDeposit, double? propertyValuation, double? feesTotal, double? interimInterest, double? monthlyInstalment, double? hOCPremium, double? lifePremium, double? preApprovedAmount, double? maxCashAllowed, double? maxQuickCashAllowed, double? requestedQuickCashAmount, double? bondToRegister, double? lTV, double? pTI)
        {
            this.OfferKey = offerKey;
            this.OfferTerm = offerTerm;
            this.CashDeposit = cashDeposit;
            this.PropertyValuation = propertyValuation;
            this.FeesTotal = feesTotal;
            this.InterimInterest = interimInterest;
            this.MonthlyInstalment = monthlyInstalment;
            this.HOCPremium = hOCPremium;
            this.LifePremium = lifePremium;
            this.PreApprovedAmount = preApprovedAmount;
            this.MaxCashAllowed = maxCashAllowed;
            this.MaxQuickCashAllowed = maxQuickCashAllowed;
            this.RequestedQuickCashAmount = requestedQuickCashAmount;
            this.BondToRegister = bondToRegister;
            this.LTV = lTV;
            this.PTI = pTI;
		
        }
		[JsonConstructor]
        public ImportOfferInformationDataModel(int offerInformationKey, int offerKey, int? offerTerm, double? cashDeposit, double? propertyValuation, double? feesTotal, double? interimInterest, double? monthlyInstalment, double? hOCPremium, double? lifePremium, double? preApprovedAmount, double? maxCashAllowed, double? maxQuickCashAllowed, double? requestedQuickCashAmount, double? bondToRegister, double? lTV, double? pTI)
        {
            this.OfferInformationKey = offerInformationKey;
            this.OfferKey = offerKey;
            this.OfferTerm = offerTerm;
            this.CashDeposit = cashDeposit;
            this.PropertyValuation = propertyValuation;
            this.FeesTotal = feesTotal;
            this.InterimInterest = interimInterest;
            this.MonthlyInstalment = monthlyInstalment;
            this.HOCPremium = hOCPremium;
            this.LifePremium = lifePremium;
            this.PreApprovedAmount = preApprovedAmount;
            this.MaxCashAllowed = maxCashAllowed;
            this.MaxQuickCashAllowed = maxQuickCashAllowed;
            this.RequestedQuickCashAmount = requestedQuickCashAmount;
            this.BondToRegister = bondToRegister;
            this.LTV = lTV;
            this.PTI = pTI;
		
        }		

        public int OfferInformationKey { get; set; }

        public int OfferKey { get; set; }

        public int? OfferTerm { get; set; }

        public double? CashDeposit { get; set; }

        public double? PropertyValuation { get; set; }

        public double? FeesTotal { get; set; }

        public double? InterimInterest { get; set; }

        public double? MonthlyInstalment { get; set; }

        public double? HOCPremium { get; set; }

        public double? LifePremium { get; set; }

        public double? PreApprovedAmount { get; set; }

        public double? MaxCashAllowed { get; set; }

        public double? MaxQuickCashAllowed { get; set; }

        public double? RequestedQuickCashAmount { get; set; }

        public double? BondToRegister { get; set; }

        public double? LTV { get; set; }

        public double? PTI { get; set; }

        public void SetKey(int key)
        {
            this.OfferInformationKey =  key;
        }
    }
}