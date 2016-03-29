using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HocFuturePremiumsDataModel :  IDataModel
    {
        public HocFuturePremiumsDataModel(int? hOCFinancialservicekey, int? hOCPolicyNumber, double? hOCMonthlyPremium, double? hOCConventionalAmount, double? hOCThatchAmount, int? mortgageLoanNumber, DateTime? currentAnniversaryDate, DateTime? effectivedate, DateTime? insertDate, double? hocAdministrationFee, double hOCBasePremium, double sASRIAAmount, int? hOCRatesKey)
        {
            this.HOCFinancialservicekey = hOCFinancialservicekey;
            this.HOCPolicyNumber = hOCPolicyNumber;
            this.HOCMonthlyPremium = hOCMonthlyPremium;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCThatchAmount = hOCThatchAmount;
            this.MortgageLoanNumber = mortgageLoanNumber;
            this.CurrentAnniversaryDate = currentAnniversaryDate;
            this.Effectivedate = effectivedate;
            this.InsertDate = insertDate;
            this.HocAdministrationFee = hocAdministrationFee;
            this.HOCBasePremium = hOCBasePremium;
            this.SASRIAAmount = sASRIAAmount;
            this.HOCRatesKey = hOCRatesKey;
		
        }		

        public int? HOCFinancialservicekey { get; set; }

        public int? HOCPolicyNumber { get; set; }

        public double? HOCMonthlyPremium { get; set; }

        public double? HOCConventionalAmount { get; set; }

        public double? HOCThatchAmount { get; set; }

        public int? MortgageLoanNumber { get; set; }

        public DateTime? CurrentAnniversaryDate { get; set; }

        public DateTime? Effectivedate { get; set; }

        public DateTime? InsertDate { get; set; }

        public double? HocAdministrationFee { get; set; }

        public double HOCBasePremium { get; set; }

        public double SASRIAAmount { get; set; }

        public int? HOCRatesKey { get; set; }
    }
}