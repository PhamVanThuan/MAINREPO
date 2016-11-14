namespace SAHL.DecisionTree.Shared
{
    public class SAHLApplicationCreditPolicy_1Variables
    {
        public SAHLApplicationCreditPolicy_1Inputs inputs = new SAHLApplicationCreditPolicy_1Inputs();
        public SAHLApplicationCreditPolicy_1Outputs outputs;

        private dynamic Enumerations;
        public SAHLApplicationCreditPolicy_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new SAHLApplicationCreditPolicy_1Outputs(Enumerations);
        }

        public class SAHLApplicationCreditPolicy_1Inputs
        {
            public int FirstIncomeContributorApplicantEmpirica { get; set; }
public double FirstIncomeContributorApplicantIncome { get; set; }
public int SecondIncomeContributorApplicantEmpirica { get; set; }
public double SecondIncomeContributorApplicantIncome { get; set; }
public double HouseholdIncome { get; set; }
public int YoungestApplicantAgeinYears { get; set; }
public int EldestApplicantAgeinYears { get; set; }

        }

        public class SAHLApplicationCreditPolicy_1Outputs
        {
            public int ApplicationEmpirica { get; set; }
public bool EligibleBorrowerAge { get; set; }
public bool EligibleEmpirica { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public SAHLApplicationCreditPolicy_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                ApplicationEmpirica = (int)0;
EligibleBorrowerAge = (bool)false;
EligibleEmpirica = (bool)false;

            }
        }
    }
}