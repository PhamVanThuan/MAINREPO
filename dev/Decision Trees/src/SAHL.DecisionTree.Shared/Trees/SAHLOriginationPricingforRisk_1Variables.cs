namespace SAHL.DecisionTree.Shared
{
    public class SAHLOriginationPricingforRisk_1Variables
    {
        public SAHLOriginationPricingforRisk_1Inputs inputs = new SAHLOriginationPricingforRisk_1Inputs();
        public SAHLOriginationPricingforRisk_1Outputs outputs;

        private dynamic Enumerations;
        public SAHLOriginationPricingforRisk_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new SAHLOriginationPricingforRisk_1Outputs(Enumerations);
        }

        public class SAHLOriginationPricingforRisk_1Inputs
        {
            public int ApplicationEmpirica { get; set; }
public string CreditMatrixCategory { get; set; }
public string HouseholdIncomeType { get; set; }

        }

        public class SAHLOriginationPricingforRisk_1Outputs
        {
            public double LinkRateAdjustment { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public SAHLOriginationPricingforRisk_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                LinkRateAdjustment = (double)0;

            }
        }
    }
}