namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentStressFactorModel
    {
        public AffordabilityAssessmentStressFactorModel()
        {
        }

        public AffordabilityAssessmentStressFactorModel(int key, string stressFactorPercentage, double percentageIncreaseOnRepayments)
        {
            this.Key = key;
            this.StressFactorPercentage = stressFactorPercentage;
            this.PercentageIncreaseOnRepayments = percentageIncreaseOnRepayments;
        }

        public int Key { get; set; }

        public string StressFactorPercentage { get; set; }

        public double PercentageIncreaseOnRepayments { get; set; }
    }
}