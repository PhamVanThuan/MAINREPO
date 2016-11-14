namespace SAHL.DecisionTree.Shared
{
    public class CapitecAffordabilityInterestRate_1Variables
    {
        public CapitecAffordabilityInterestRate_1Inputs inputs = new CapitecAffordabilityInterestRate_1Inputs();
        public CapitecAffordabilityInterestRate_1Outputs outputs;

        private dynamic Enumerations;
        public CapitecAffordabilityInterestRate_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new CapitecAffordabilityInterestRate_1Outputs(Enumerations);
        }

        public class CapitecAffordabilityInterestRate_1Inputs
        {
            public double HouseholdIncome { get; set; }
public double Deposit { get; set; }
public double CalcRate { get; set; }
public bool InterestRateQuery { get; set; }

        }

        public class CapitecAffordabilityInterestRate_1Outputs
        {
            public double InterestRate { get; set; }
public double AmountQualifiedFor { get; set; }
public double PropertyPriceQualifiedFor { get; set; }
public double PaymentToIncome { get; set; }
public int TermInMonths { get; set; }
public double Instalment { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public CapitecAffordabilityInterestRate_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                InterestRate = (double)0;
AmountQualifiedFor = (double)0;
PropertyPriceQualifiedFor = (double)0;
PaymentToIncome = (double)0;
TermInMonths = (int)0;
Instalment = (double)0;

            }
        }
    }
}