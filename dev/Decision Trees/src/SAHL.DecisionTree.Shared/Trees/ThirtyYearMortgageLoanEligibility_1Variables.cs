namespace SAHL.DecisionTree.Shared
{
    public class ThirtyYearMortgageLoanEligibility_1Variables
    {
        public ThirtyYearMortgageLoanEligibility_1Inputs inputs = new ThirtyYearMortgageLoanEligibility_1Inputs();
        public ThirtyYearMortgageLoanEligibility_1Outputs outputs;

        private dynamic Enumerations;
        public ThirtyYearMortgageLoanEligibility_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new ThirtyYearMortgageLoanEligibility_1Outputs(Enumerations);
        }

        public class ThirtyYearMortgageLoanEligibility_1Inputs
        {
            public bool ApplicationBlockedByCredit { get; set; }
public string Product { get; set; }
public string HighestIncomeContributorSalaryType { get; set; }
public int HighestIncomeContributorAge { get; set; }
public double HouseholdIncome { get; set; }
public int HighestIncomeContributorCreditScore { get; set; }
public double CurrentLTV { get; set; }
public double CurrentPTI { get; set; }
public string MortgageLoanApplicationType { get; set; }
public double LoanAmount { get; set; }
public double PropertyValue { get; set; }
public double InterestRate { get; set; }
public string HighestIncomeContributorName { get; set; }
public string HighestIncomeContributorIdentity { get; set; }
public bool IsAlpha { get; set; }
public bool InterestOnly { get; set; }

        }

        public class ThirtyYearMortgageLoanEligibility_1Outputs
        {
            public bool QualifiesForThirtyYearLoanTerm { get; set; }
public double InterestRateThirtyYear { get; set; }
public double LoantoValueThirtyYear { get; set; }
public double PaymenttoIncomeThirtyYear { get; set; }
public double InstalmentThirtyYear { get; set; }
public double PricingAdjustmentThirtyYear { get; set; }
public int TermThirtyYear { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public ThirtyYearMortgageLoanEligibility_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                QualifiesForThirtyYearLoanTerm = (bool)false;
TermThirtyYear = (int)0;

            }
        }
    }
}