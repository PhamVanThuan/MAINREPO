namespace SAHL.DecisionTree.Shared
{
    public class CapitecOriginationPricingforRisk_2Variables
    {
        public CapitecOriginationPricingforRisk_2Inputs inputs = new CapitecOriginationPricingforRisk_2Inputs();
        public CapitecOriginationPricingforRisk_2Outputs outputs;

        private dynamic Enumerations;
        public CapitecOriginationPricingforRisk_2Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new CapitecOriginationPricingforRisk_2Outputs(Enumerations);
        }

        public class CapitecOriginationPricingforRisk_2Inputs
        {
            public int ApplicationEmpirica { get; set; }
public string CreditMatrixCategory { get; set; }
public string HouseholdIncomeType { get; set; }

        }

        public class CapitecOriginationPricingforRisk_2Outputs
        {
            public double LinkRateAdjustment { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public CapitecOriginationPricingforRisk_2Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                LinkRateAdjustment = (double)0;

            }
        }
    }
}