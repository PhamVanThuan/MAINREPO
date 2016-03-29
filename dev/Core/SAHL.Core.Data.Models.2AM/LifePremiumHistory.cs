using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePremiumHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifePremiumHistoryDataModel(DateTime? changeDate, double deathPremium, double iPBPremium, double sumAssured, double yearlyPremium, double policyFactor, double discountFactor, string userName, int accountKey, double? monthlyPremium)
        {
            this.ChangeDate = changeDate;
            this.DeathPremium = deathPremium;
            this.IPBPremium = iPBPremium;
            this.SumAssured = sumAssured;
            this.YearlyPremium = yearlyPremium;
            this.PolicyFactor = policyFactor;
            this.DiscountFactor = discountFactor;
            this.UserName = userName;
            this.AccountKey = accountKey;
            this.MonthlyPremium = monthlyPremium;
		
        }
		[JsonConstructor]
        public LifePremiumHistoryDataModel(int lifePremiumHistoryKey, DateTime? changeDate, double deathPremium, double iPBPremium, double sumAssured, double yearlyPremium, double policyFactor, double discountFactor, string userName, int accountKey, double? monthlyPremium)
        {
            this.LifePremiumHistoryKey = lifePremiumHistoryKey;
            this.ChangeDate = changeDate;
            this.DeathPremium = deathPremium;
            this.IPBPremium = iPBPremium;
            this.SumAssured = sumAssured;
            this.YearlyPremium = yearlyPremium;
            this.PolicyFactor = policyFactor;
            this.DiscountFactor = discountFactor;
            this.UserName = userName;
            this.AccountKey = accountKey;
            this.MonthlyPremium = monthlyPremium;
		
        }		

        public int LifePremiumHistoryKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public double DeathPremium { get; set; }

        public double IPBPremium { get; set; }

        public double SumAssured { get; set; }

        public double YearlyPremium { get; set; }

        public double PolicyFactor { get; set; }

        public double DiscountFactor { get; set; }

        public string UserName { get; set; }

        public int AccountKey { get; set; }

        public double? MonthlyPremium { get; set; }

        public void SetKey(int key)
        {
            this.LifePremiumHistoryKey =  key;
        }
    }
}