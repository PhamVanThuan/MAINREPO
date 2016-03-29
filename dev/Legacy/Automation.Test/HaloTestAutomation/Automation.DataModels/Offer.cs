using Common.Enums;

namespace Automation.DataModels
{
    public sealed class Offer : IDataModel
    {
        public float ProfitFromSale { get; set; }

        public float MonthlyInstalment { get; set; }

        public int TermMonths { get; set; }

        public int TermYears { get; set; }

        public double InterestRate { get; set; }

        public int NoApplicants { get; set; }

        public int OfferKey { get; set; }

        public string OrigConsultant { get; set; }

        public string ReAssignedConsultant { get; set; }

        public float OtherContributions { get; set; }

        public float ExistingLoan { get; set; }

        public OfferTypeEnum OfferTypeKey { get; set; }

        public OfferTypeEnum OfferType { get; set; }

        public float ClientEstimatePropertyValuation { get; set; }

        public double LTV { get; set; }

        public double FixedPercentage { get; set; }

        public double PTI { get; set; }

        public OfferStatusEnum OfferStatus { get; set; }

        public float LoanAgreementAmount { get; set; }

        public float HouseholdIncome { get; set; }

        public bool IsInterestOnly { get; set; }

        public bool IsCapitaliseFees { get; set; }

        public float CashOut { get; set; }

        public ProductEnum ProductType { get; set; }

        public MortgageLoanPurposeEnum LoanPurpose { get; set; }

        public float PurchasePrice { get; set; }

        public float CashDeposit { get; set; }

        public int AccountKey { get; set; }

        public int LegalEntityKey { get; set; }

        public OfferAttribute OfferAttribute { get; set; }
    }
}