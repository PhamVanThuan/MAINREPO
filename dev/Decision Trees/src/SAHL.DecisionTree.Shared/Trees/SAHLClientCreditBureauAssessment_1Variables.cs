namespace SAHL.DecisionTree.Shared
{
    public class SAHLClientCreditBureauAssessment_1Variables
    {
        public SAHLClientCreditBureauAssessment_1Inputs inputs = new SAHLClientCreditBureauAssessment_1Inputs();
        public SAHLClientCreditBureauAssessment_1Outputs outputs;

        private dynamic Enumerations;
        public SAHLClientCreditBureauAssessment_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new SAHLClientCreditBureauAssessment_1Outputs(Enumerations);
        }

        public class SAHLClientCreditBureauAssessment_1Inputs
        {
            public int ApplicantEmpirica { get; set; }
public int NumberofJudgmentswithinLast3Years { get; set; }
public double AggregatedJudgmentValuewithinLast3Years { get; set; }
public double NonSettledAggregatedJudgmentValuewithinLast3Years { get; set; }
public int NumberofUnsettledDefaultswithinLast2Years { get; set; }
public bool SequestrationNotice { get; set; }
public bool AdministrationOrderNotice { get; set; }
public bool DebtCounsellingNotice { get; set; }
public bool DebtReviewNotice { get; set; }
public bool ConsumerDeceasedNotification { get; set; }
public bool CreditCardRevoked { get; set; }
public bool ConsumerAbsconded { get; set; }
public bool PaidOutonDeceasedClaim { get; set; }
public bool CreditBureauMatch { get; set; }

        }

        public class SAHLClientCreditBureauAssessment_1Outputs
        {
            public bool EligibleBorrower { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public SAHLClientCreditBureauAssessment_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                EligibleBorrower = (bool)false;

            }
        }
    }
}