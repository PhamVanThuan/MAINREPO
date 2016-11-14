namespace SAHL.DecisionTree.Shared
{
    public class CapitecOriginationPricingforRisk_1Variables
    {
        public CapitecOriginationPricingforRisk_1Inputs inputs = new CapitecOriginationPricingforRisk_1Inputs();
        public CapitecOriginationPricingforRisk_1Outputs outputs;

        private dynamic Enumerations;
        public CapitecOriginationPricingforRisk_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new CapitecOriginationPricingforRisk_1Outputs(Enumerations);
        }

        public class CapitecOriginationPricingforRisk_1Inputs
        {
            public int ApplicationEmpirica { get; set; }
public string CreditMatrixCategory { get; set; }
public string HouseholdIncomeType { get; set; }

        }

        public class CapitecOriginationPricingforRisk_1Outputs
        {
            public double LinkRateAdjustment { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public CapitecOriginationPricingforRisk_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                LinkRateAdjustment = (double)0;

            }
        }
    }
}