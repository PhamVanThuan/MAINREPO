using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetAssessmentStressFactorByPercentageStatement : ISqlStatement<AffordabilityAssessmentStressFactorDataModel>
    {
        public GetAssessmentStressFactorByPercentageStatement(string stressFactorPercentage)
        {
            this.StressFactorPercentage = stressFactorPercentage;
        }

        public string StressFactorPercentage { get; protected set; }

        public string GetStatement()
        {
            return @"SELECT [AffordabilityAssessmentStressFactorKey]
                          , [StressFactorPercentage]
                          , [PercentageIncreaseOnRepayments]
                      FROM [2AM].[dbo].[AffordabilityAssessmentStressFactor]
                      WHERE [StressFactorPercentage] = @StressFactorPercentage";
        }
    }
}