using System;

namespace Automation.DataModels
{
    public class PersonalLoanApplication
    {
        public int OfferKey { get; set; }

        public int OfferStatusKey { get; set; }

        public DateTime OfferStartDate { get; set; }

        public int ReservedAccountKey { get; set; }

        public double LoanAmount { get; set; }

        public int Term { get; set; }

        public double MonthlyInstalment { get; set; }

        public double LifePremium { get; set; }

        public double FeesTotal { get; set; }

        public double LinkRate { get; set; }

        public double MarketRate { get; set; }

        public double TotalRate { get; set; }

        public int OfferInformationTypeKey { get; set; }

        public int CreditLifeTakenUp { get; set; }
    }
}