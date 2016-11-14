namespace SAHL.DecisionTree.Shared
{
    public class CapitecAlphaCreditPricing_1Variables
    {
        public CapitecAlphaCreditPricing_1Inputs inputs = new CapitecAlphaCreditPricing_1Inputs();
        public CapitecAlphaCreditPricing_1Outputs outputs;

        private dynamic Enumerations;
        public CapitecAlphaCreditPricing_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new CapitecAlphaCreditPricing_1Outputs(Enumerations);
        }

        public class CapitecAlphaCreditPricing_1Inputs
        {
            public string ApplicationType { get; set; }
public string PropertyOccupancyType { get; set; }
public string HouseholdIncomeType { get; set; }
public double HouseholdIncome { get; set; }
public double PropertyPurchasePrice { get; set; }
public double DepositAmount { get; set; }
public double CashAmountRequired { get; set; }
public double CurrentMortgageLoanBalance { get; set; }
public double EstimatedMarketValueofProperty { get; set; }
public int TermInMonth { get; set; }
public int ApplicationEmpirica { get; set; }
public double Fees { get; set; }
public double InterimInterest { get; set; }
public bool CapitaliseFees { get; set; }

        }

        public class CapitecAlphaCreditPricing_1Outputs
        {
            public double InterestRate { get; set; }
public double LoantoValue { get; set; }
public double LoanAmount { get; set; }
public double PaymenttoIncome { get; set; }
public string CreditMatrixCategory { get; set; }
public bool Alpha { get; set; }
public double PropertyValue { get; set; }
public double Instalment { get; set; }
public bool EligibleApplication { get; set; }
public string InterestRateasPercent { get; set; }
public string LoantoValueasPercent { get; set; }
public string PaymenttoIncomeasPercent { get; set; }
public string InstallmentinRands { get; set; }
public double LinkRate { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public CapitecAlphaCreditPricing_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                InterestRate = (double)0;
LoantoValue = (double)0;
LoanAmount = (double)0;
PaymenttoIncome = (double)0;
CreditMatrixCategory = Enumerations.sAHomeLoans.defaultEnumValue.Unknown;
Alpha = (bool)false;
PropertyValue = (double)0;
Instalment = (double)0;
EligibleApplication = (bool)false;
InterestRateasPercent = string.Empty;
LoantoValueasPercent = string.Empty;
PaymenttoIncomeasPercent = string.Empty;
InstallmentinRands = string.Empty;
LinkRate = (double)0;

            }
        }
    }
}