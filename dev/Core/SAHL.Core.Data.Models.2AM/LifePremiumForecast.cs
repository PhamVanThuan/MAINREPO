using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePremiumForecastDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifePremiumForecastDataModel(short loanYear, short age, double sumAssured, double monthlyPremium, double monthlyComm, DateTime entryDate, int accountKey, double? yearlyPremium, decimal? iPBPremium)
        {
            this.LoanYear = loanYear;
            this.Age = age;
            this.SumAssured = sumAssured;
            this.MonthlyPremium = monthlyPremium;
            this.MonthlyComm = monthlyComm;
            this.EntryDate = entryDate;
            this.AccountKey = accountKey;
            this.YearlyPremium = yearlyPremium;
            this.IPBPremium = iPBPremium;
		
        }
		[JsonConstructor]
        public LifePremiumForecastDataModel(int lifePremiumForecastKey, short loanYear, short age, double sumAssured, double monthlyPremium, double monthlyComm, DateTime entryDate, int accountKey, double? yearlyPremium, decimal? iPBPremium)
        {
            this.LifePremiumForecastKey = lifePremiumForecastKey;
            this.LoanYear = loanYear;
            this.Age = age;
            this.SumAssured = sumAssured;
            this.MonthlyPremium = monthlyPremium;
            this.MonthlyComm = monthlyComm;
            this.EntryDate = entryDate;
            this.AccountKey = accountKey;
            this.YearlyPremium = yearlyPremium;
            this.IPBPremium = iPBPremium;
		
        }		

        public int LifePremiumForecastKey { get; set; }

        public short LoanYear { get; set; }

        public short Age { get; set; }

        public double SumAssured { get; set; }

        public double MonthlyPremium { get; set; }

        public double MonthlyComm { get; set; }

        public DateTime EntryDate { get; set; }

        public int AccountKey { get; set; }

        public double? YearlyPremium { get; set; }

        public decimal? IPBPremium { get; set; }

        public void SetKey(int key)
        {
            this.LifePremiumForecastKey =  key;
        }
    }
}