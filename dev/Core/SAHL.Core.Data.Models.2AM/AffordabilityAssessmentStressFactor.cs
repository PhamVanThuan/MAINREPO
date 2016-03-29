using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityAssessmentStressFactorDataModel :  IDataModel
    {
        public AffordabilityAssessmentStressFactorDataModel(int affordabilityAssessmentStressFactorKey, string stressFactorPercentage, decimal percentageIncreaseOnRepayments)
        {
            this.AffordabilityAssessmentStressFactorKey = affordabilityAssessmentStressFactorKey;
            this.StressFactorPercentage = stressFactorPercentage;
            this.PercentageIncreaseOnRepayments = percentageIncreaseOnRepayments;
		
        }		

        public int AffordabilityAssessmentStressFactorKey { get; set; }

        public string StressFactorPercentage { get; set; }

        public decimal PercentageIncreaseOnRepayments { get; set; }
    }
}