using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationPersonalLoanDataModel :  IDataModel
    {
        public OfferInformationPersonalLoanDataModel(int offerInformationKey, double loanAmount, int term, double monthlyInstalment, double? lifePremium, double feesTotal, int creditCriteriaUnsecuredLendingKey, int marginKey, int marketRateKey)
        {
            this.OfferInformationKey = offerInformationKey;
            this.LoanAmount = loanAmount;
            this.Term = term;
            this.MonthlyInstalment = monthlyInstalment;
            this.LifePremium = lifePremium;
            this.FeesTotal = feesTotal;
            this.CreditCriteriaUnsecuredLendingKey = creditCriteriaUnsecuredLendingKey;
            this.MarginKey = marginKey;
            this.MarketRateKey = marketRateKey;
		
        }		

        public int OfferInformationKey { get; set; }

        public double LoanAmount { get; set; }

        public int Term { get; set; }

        public double MonthlyInstalment { get; set; }

        public double? LifePremium { get; set; }

        public double FeesTotal { get; set; }

        public int CreditCriteriaUnsecuredLendingKey { get; set; }

        public int MarginKey { get; set; }

        public int MarketRateKey { get; set; }
    }
}