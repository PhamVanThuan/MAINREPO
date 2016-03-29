using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RegistrationLetterInfoDataModel :  IDataModel
    {
        public RegistrationLetterInfoDataModel(int accountKey, string openDate, string description, string hOCPolicyNumber, double? monthlyInstallment, double? monthlyPremium, double? hOCProRataPremium, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, double? hOCTotalSumInsured, string nextinstalmentdate, double? initialBalance)
        {
            this.AccountKey = accountKey;
            this.OpenDate = openDate;
            this.Description = description;
            this.HOCPolicyNumber = hOCPolicyNumber;
            this.MonthlyInstallment = monthlyInstallment;
            this.MonthlyPremium = monthlyPremium;
            this.HOCProRataPremium = hOCProRataPremium;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.HOCTotalSumInsured = hOCTotalSumInsured;
            this.Nextinstalmentdate = nextinstalmentdate;
            this.InitialBalance = initialBalance;
		
        }		

        public int AccountKey { get; set; }

        public string OpenDate { get; set; }

        public string Description { get; set; }

        public string HOCPolicyNumber { get; set; }

        public double? MonthlyInstallment { get; set; }

        public double? MonthlyPremium { get; set; }

        public double? HOCProRataPremium { get; set; }

        public double? HOCThatchAmount { get; set; }

        public double? HOCConventionalAmount { get; set; }

        public double? HOCShingleAmount { get; set; }

        public double? HOCTotalSumInsured { get; set; }

        public string Nextinstalmentdate { get; set; }

        public double? InitialBalance { get; set; }
    }
}