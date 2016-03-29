using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePolicyDataModel :  IDataModel
    {
        public LifePolicyDataModel(int financialServiceKey, double deathBenefit, double installmentProtectionBenefit, double deathBenefitPremium, double installmentProtectionPremium, int policyStatusKey, DateTime? dateOfCommencement, DateTime dateOfExpiry, double deathRetentionLimit, double installmentProtectionRetentionLimit, decimal upliftFactor, decimal jointDiscountFactor, DateTime? dateOfCancellation, double yearlyPremium, DateTime? dateOfAcceptance, double? sumAssured, DateTime? dateLastUpdated, string consultant, int? claimStatusKey, double? currentSumAssured, double? premiumShortfall, int? insurerKey, string externalPolicyNumber, DateTime? dateCeded, int? priorityKey, DateTime? claimStatusDate, int? policyHolderLEKey, string rPARInsurer, string rPARPolicyNumber, int? brokerKey, double? deathReassuranceRetention, double? iPBReassuranceRetention, int lifePolicyTypeKey, DateTime? anniversaryDate, int? claimTypeKey)
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
            this.YearlyPremium = yearlyPremium;
            this.DateOfAcceptance = dateOfAcceptance;
            this.SumAssured = sumAssured;
            this.DateLastUpdated = dateLastUpdated;
            this.Consultant = consultant;
            this.ClaimStatusKey = claimStatusKey;
            this.CurrentSumAssured = currentSumAssured;
            this.PremiumShortfall = premiumShortfall;
            this.InsurerKey = insurerKey;
            this.ExternalPolicyNumber = externalPolicyNumber;
            this.DateCeded = dateCeded;
            this.PriorityKey = priorityKey;
            this.ClaimStatusDate = claimStatusDate;
            this.PolicyHolderLEKey = policyHolderLEKey;
            this.RPARInsurer = rPARInsurer;
            this.RPARPolicyNumber = rPARPolicyNumber;
            this.BrokerKey = brokerKey;
            this.DeathReassuranceRetention = deathReassuranceRetention;
            this.IPBReassuranceRetention = iPBReassuranceRetention;
            this.LifePolicyTypeKey = lifePolicyTypeKey;
            this.AnniversaryDate = anniversaryDate;
            this.ClaimTypeKey = claimTypeKey;
		
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

        public double YearlyPremium { get; set; }

        public DateTime? DateOfAcceptance { get; set; }

        public double? SumAssured { get; set; }

        public DateTime? DateLastUpdated { get; set; }

        public string Consultant { get; set; }

        public int? ClaimStatusKey { get; set; }

        public double? CurrentSumAssured { get; set; }

        public double? PremiumShortfall { get; set; }

        public int? InsurerKey { get; set; }

        public string ExternalPolicyNumber { get; set; }

        public DateTime? DateCeded { get; set; }

        public int? PriorityKey { get; set; }

        public DateTime? ClaimStatusDate { get; set; }

        public int? PolicyHolderLEKey { get; set; }

        public string RPARInsurer { get; set; }

        public string RPARPolicyNumber { get; set; }

        public int? BrokerKey { get; set; }

        public double? DeathReassuranceRetention { get; set; }

        public double? IPBReassuranceRetention { get; set; }

        public int LifePolicyTypeKey { get; set; }

        public DateTime? AnniversaryDate { get; set; }

        public int? ClaimTypeKey { get; set; }
    }
}