using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferLifeDataModel :  IDataModel
    {
        public OfferLifeDataModel(int offerKey, double deathBenefit, double installmentProtectionBenefit, double deathBenefitPremium, double installmentProtectionPremium, DateTime dateOfExpiry, decimal upliftFactor, decimal jointDiscountFactor, double monthlyPremium, double yearlyPremium, double? sumAssured, DateTime? dateLastUpdated, double? currentSumAssured, double? premiumShortfall, int? insurerKey, string externalPolicyNumber, DateTime? dateCeded, int? priorityKey, int? policyHolderLEKey, string rPARInsurer, string rPARPolicyNumber, DateTime? dateOfAcceptance, string consultant, int lifePolicyTypeKey)
        {
            this.OfferKey = offerKey;
            this.DeathBenefit = deathBenefit;
            this.InstallmentProtectionBenefit = installmentProtectionBenefit;
            this.DeathBenefitPremium = deathBenefitPremium;
            this.InstallmentProtectionPremium = installmentProtectionPremium;
            this.DateOfExpiry = dateOfExpiry;
            this.UpliftFactor = upliftFactor;
            this.JointDiscountFactor = jointDiscountFactor;
            this.MonthlyPremium = monthlyPremium;
            this.YearlyPremium = yearlyPremium;
            this.SumAssured = sumAssured;
            this.DateLastUpdated = dateLastUpdated;
            this.CurrentSumAssured = currentSumAssured;
            this.PremiumShortfall = premiumShortfall;
            this.InsurerKey = insurerKey;
            this.ExternalPolicyNumber = externalPolicyNumber;
            this.DateCeded = dateCeded;
            this.PriorityKey = priorityKey;
            this.PolicyHolderLEKey = policyHolderLEKey;
            this.RPARInsurer = rPARInsurer;
            this.RPARPolicyNumber = rPARPolicyNumber;
            this.DateOfAcceptance = dateOfAcceptance;
            this.Consultant = consultant;
            this.LifePolicyTypeKey = lifePolicyTypeKey;
		
        }		

        public int OfferKey { get; set; }

        public double DeathBenefit { get; set; }

        public double InstallmentProtectionBenefit { get; set; }

        public double DeathBenefitPremium { get; set; }

        public double InstallmentProtectionPremium { get; set; }

        public DateTime DateOfExpiry { get; set; }

        public decimal UpliftFactor { get; set; }

        public decimal JointDiscountFactor { get; set; }

        public double MonthlyPremium { get; set; }

        public double YearlyPremium { get; set; }

        public double? SumAssured { get; set; }

        public DateTime? DateLastUpdated { get; set; }

        public double? CurrentSumAssured { get; set; }

        public double? PremiumShortfall { get; set; }

        public int? InsurerKey { get; set; }

        public string ExternalPolicyNumber { get; set; }

        public DateTime? DateCeded { get; set; }

        public int? PriorityKey { get; set; }

        public int? PolicyHolderLEKey { get; set; }

        public string RPARInsurer { get; set; }

        public string RPARPolicyNumber { get; set; }

        public DateTime? DateOfAcceptance { get; set; }

        public string Consultant { get; set; }

        public int LifePolicyTypeKey { get; set; }
    }
}