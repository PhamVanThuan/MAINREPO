namespace SAHL.DecisionTree.Shared
{
    public class NCRAffordabilityAssessment_1Variables
    {
        public NCRAffordabilityAssessment_1Inputs inputs = new NCRAffordabilityAssessment_1Inputs();
        public NCRAffordabilityAssessment_1Outputs outputs;

        private dynamic Enumerations;
        public NCRAffordabilityAssessment_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new NCRAffordabilityAssessment_1Outputs(Enumerations);
        }

        public class NCRAffordabilityAssessment_1Inputs
        {
            public double GrossClientMonthlyIncome { get; set; }

        }

        public class NCRAffordabilityAssessment_1Outputs
        {
            public double MinMonthlyFixedExpenses { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public NCRAffordabilityAssessment_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                MinMonthlyFixedExpenses = (double)0;

            }
        }
    }
}