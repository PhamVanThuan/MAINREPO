using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePolicy_SnapshotDataModel :  IDataModel
    {
        public LifePolicy_SnapshotDataModel(int financialServiceKey, double deathBenefit, double installmentProtectionBenefit, double deathBenefitPremium, double installmentProtectionPremium, int policyStatusKey, DateTime? dateOfCommencement, DateTime dateOfExpiry, double deathRetentionLimit, double installmentProtectionRetentionLimit, decimal upliftFactor, decimal jointDiscountFactor, DateTime? dateOfCancellation, double? currentArrearBalance, double? currentYearlyBalance, double monthlyPremium, double yearlyPremium, double? capitalizedYearlyBalance, double? capitalizedMonthlyBalance, DateTime? dateOfAcceptance, double? sumAssured, DateTime? dateLastUpdated, string consultant, int? claimStatusKey, double? currentSumAssured, double? premiumShortfall)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.DeathBenefit = deathBenefit;
            this.InstallmentProtectionBenefit = installmentProtectionBenefit;
            this.DeathBenefitPremium = deathBenefitPremium;
            this.InstallmentProtectionPremium = installmentProtectionPremium;
            this.PolicyStatusKey = policyStatusKey;
            this.DateOfCommencement = dateOfCommencement;
            this.DateOfExpiry = dateOfExpiry;
            this.DeathRetentionLimit = deathRetentionLimit;
            this.InstallmentProtectionRetentionLimit = installmentProtectionRetentionLimit;
            this.UpliftFactor = upliftFactor;
            this.JointDiscountFactor = jointDiscountFactor;
            this.DateOfCancellation = dateOfCancellation;
            this.CurrentArrearBalance = currentArrearBalance;
            this.CurrentYearlyBalance = currentYearlyBalance;
            this.MonthlyPremium = monthlyPremium;
            this.YearlyPremium = yearlyPremium;
            this.CapitalizedYearlyBalance = capitalizedYearlyBalance;
            this.CapitalizedMonthlyBalance = capitalizedMonthlyBalance;
            this.DateOfAcceptance = dateOfAcceptance;
            this.SumAssured = sumAssured;
            this.DateLastUpdated = dateLastUpdated;
            this.Consultant = consultant;
            this.ClaimStatusKey = claimStatusKey;
            this.CurrentSumAssured = currentSumAssured;
            this.PremiumShortfall = premiumShortfall;
		
        }		

        public int FinancialServiceKey { get; set; }

        public double DeathBenefit { get; set; }

        public double InstallmentProtectionBenefit { get; set; }

        public double DeathBenefitPremium { get; set; }

        public double InstallmentProtectionPremium { get; set; }

        public int PolicyStatusKey { get; set; }

        public DateTime? DateOfCommencement { get; set; }

        public DateTime DateOfExpiry { get; set; }

        public double DeathRetentionLimit { get; set; }

        public double InstallmentProtectionRetentionLimit { get; set; }

        public decimal UpliftFactor { get; set; }

        public decimal JointDiscountFactor { get; set; }

        public DateTime? DateOfCancellation { get; set; }

        public double? CurrentArrearBalance { get; set; }

        public double? CurrentYearlyBalance { get; set; }

        public double MonthlyPremium { get; set; }

        public double YearlyPremium { get; set; }

        public double? CapitalizedYearlyBalance { get; set; }

        public double? CapitalizedMonthlyBalance { get; set; }

        public DateTime? DateOfAcceptance { get; set; }

        public double? SumAssured { get; set; }

        public DateTime? DateLastUpdated { get; set; }

        public string Consultant { get; set; }

        public int? ClaimStatusKey { get; set; }

        public double? CurrentSumAssured { get; set; }

        public double? PremiumShortfall { get; set; }
    }
}