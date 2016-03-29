using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentSummaryModel
    {
        public AffordabilityAssessmentSummaryModel()
        {
        }

        public AffordabilityAssessmentSummaryModel(int key,
            int genericKey,
            string clientDetail,
            int affordabilityAssessmentStatusKey,
            string assessmentStatus,
            string userLastAmended,
            DateTime dateLastAmended,
            int householdDependants,
            int contributingApplicants,
            int stressFactorKey,
            string stressFactorPercentageDisplay,
            double percentageIncreaseOnRepayments,
            int? minimumMonthlyFixedExpenses,
            DateTime? dateConfirmed,
            int generalStatusKey)
        {
            this.Key = key;
            this.GenericKey = genericKey;
            this.ClientDetail = clientDetail;
            this.AffordabilityAssessmentStatusKey = affordabilityAssessmentStatusKey;
            this.AssessmentStatus = assessmentStatus;
            this.UserLastAmended = userLastAmended;
            this.DateLastAmended = dateLastAmended;
            this.HouseholdDependants = householdDependants;
            this.ContributingApplicants = contributingApplicants;
            this.StressFactorKey = stressFactorKey;
            this.StressFactorPercentageDisplay = stressFactorPercentageDisplay;
            this.PercentageIncreaseOnRepayments = percentageIncreaseOnRepayments;
            this.MinimumMonthlyFixedExpenses = minimumMonthlyFixedExpenses;
            this.DateConfirmed = dateConfirmed;
            this.GeneralStatusKey = generalStatusKey;
        }

        public int Key { get; set; }

        public int GenericKey { get; set; }

        public DateTime DateLastAmended { get; set; }

        public string ClientDetail { get; set; }

        public int AffordabilityAssessmentStatusKey { get; set; }

        public string AssessmentStatus { get; set; }

        public string UserLastAmended { get; set; }

        public int HouseholdDependants { get; set; }

        public int ContributingApplicants { get; set; }

        public int StressFactorKey { get; set; }

        public string StressFactorPercentageDisplay { get; set; }

        public double PercentageIncreaseOnRepayments { get; set; }

        public DateTime? DateConfirmed { get; set; }

        public int? MinimumMonthlyFixedExpenses { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}