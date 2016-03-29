using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MonthEndInterestDataModel :  IDataModel
    {
        public MonthEndInterestDataModel(int financialServiceKey, double? accruedInterest, double? loyaltyBenifit, double? coPayment, double? accruedInterestNew, double? loyaltyBenefitNew, double? coPaymentNew, double? acruedInterestMonthEnd, double? coPaymentMonthend)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.AccruedInterest = accruedInterest;
            this.LoyaltyBenifit = loyaltyBenifit;
            this.CoPayment = coPayment;
            this.AccruedInterestNew = accruedInterestNew;
            this.LoyaltyBenefitNew = loyaltyBenefitNew;
            this.CoPaymentNew = coPaymentNew;
            this.AcruedInterestMonthEnd = acruedInterestMonthEnd;
            this.CoPaymentMonthend = coPaymentMonthend;
		
        }		

        public int FinancialServiceKey { get; set; }

        public double? AccruedInterest { get; set; }

        public double? LoyaltyBenifit { get; set; }

        public double? CoPayment { get; set; }

        public double? AccruedInterestNew { get; set; }

        public double? LoyaltyBenefitNew { get; set; }

        public double? CoPaymentNew { get; set; }

        public double? AcruedInterestMonthEnd { get; set; }

        public double? CoPaymentMonthend { get; set; }
    }
}