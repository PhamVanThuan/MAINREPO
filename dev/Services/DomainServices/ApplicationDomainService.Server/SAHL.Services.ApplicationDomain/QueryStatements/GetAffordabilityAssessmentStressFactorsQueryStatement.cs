using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetAffordabilityAssessmentStressFactorsQueryStatement : IServiceQuerySqlStatement<GetAffordabilityAssessmentStressFactorsQuery, AffordabilityAssessmentStressFactorModel>
    {
        public string GetStatement()
        {
            return @"select
                        [AffordabilityAssessmentStressFactorKey] as 'Key',
                        [StressFactorPercentage],
                        [PercentageIncreaseOnRepayments]
                    from
                        [2am].[dbo].[AffordabilityAssessmentStressFactor]
                    order by
                        PercentageIncreaseOnRepayments";
        }
    }
}