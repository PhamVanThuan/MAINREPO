using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferMortgageLoanDataModel :  IDataModel
    {
        public OfferMortgageLoanDataModel(int offerKey, double? offerAmount, int? mortgageLoanPurposeKey, int? applicantTypeKey, int? numApplicants, DateTime? homePurchaseDate, DateTime? bondRegistrationDate, double? currentBondValue, DateTime? deedsOfficeDate, string bondFinancialInstitution, double? purchasePrice, int? resetConfigurationKey, string transferringAttorney, double? clientEstimatePropertyValuation, int? propertyKey, int? dependentsPerHousehold, int? contributingDependents, int documentLanguageKey)
        {
            this.OfferKey = offerKey;
            this.OfferAmount = offerAmount;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.ApplicantTypeKey = applicantTypeKey;
            this.NumApplicants = numApplicants;
            this.HomePurchaseDate = homePurchaseDate;
            this.BondRegistrationDate = bondRegistrationDate;
            this.CurrentBondValue = currentBondValue;
            this.DeedsOfficeDate = deedsOfficeDate;
            this.BondFinancialInstitution = bondFinancialInstitution;
            this.PurchasePrice = purchasePrice;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.TransferringAttorney = transferringAttorney;
            this.ClientEstimatePropertyValuation = clientEstimatePropertyValuation;
            this.PropertyKey = propertyKey;
            this.DependentsPerHousehold = dependentsPerHousehold;
            this.ContributingDependents = contributingDependents;
            this.DocumentLanguageKey = documentLanguageKey;
		
        }		

        public int OfferKey { get; set; }

        public double? OfferAmount { get; set; }

        public int? MortgageLoanPurposeKey { get; set; }

        public int? ApplicantTypeKey { get; set; }

        public int? NumApplicants { get; set; }

        public DateTime? HomePurchaseDate { get; set; }

        public DateTime? BondRegistrationDate { get; set; }

        public double? CurrentBondValue { get; set; }

        public DateTime? DeedsOfficeDate { get; set; }

        public string BondFinancialInstitution { get; set; }

        public double? PurchasePrice { get; set; }

        public int? ResetConfigurationKey { get; set; }

        public string TransferringAttorney { get; set; }

        public double? ClientEstimatePropertyValuation { get; set; }

        public int? PropertyKey { get; set; }

        public int? DependentsPerHousehold { get; set; }

        public int? ContributingDependents { get; set; }

        public int DocumentLanguageKey { get; set; }
    }
}