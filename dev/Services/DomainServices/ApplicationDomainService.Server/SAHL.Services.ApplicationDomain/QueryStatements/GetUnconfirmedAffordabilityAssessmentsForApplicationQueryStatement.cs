using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetUnconfirmedAffordabilityAssessmentsForApplicationQueryStatement
        : IServiceQuerySqlStatement<GetUnconfirmedAffordabilityAssessmentsForApplicationQuery, AffordabilityAssessmentSummaryModel>
    {
        public string GetStatement()
        {
            return @"select
	                    aa.AffordabilityAssessmentKey as 'Key',
						aa.ModifiedDate as 'DateLastAmended',
						[dbo].[fGetAffordabilityAssessmentContributors] (aa.AffordabilityAssessmentKey,1,0) as 'ClientDetail',
						ast.[Description] as 'AssessmentStatus',
						aa.ConfirmedDate as 'DateConfirmed'
                    from
	                    [2AM].[dbo].[AffordabilityAssessment] aa
                    join
	                    [2AM].[dbo].AffordabilityAssessmentStatus ast
                    on
	                    ast.AffordabilityAssessmentStatusKey = aa.AffordabilityAssessmentStatusKey
                    where
	                    aa.GenericKey = @ApplicationKey
	                    and aa.GenericKeyTypeKey = 2
	                    and aa.GeneralStatusKey = 1
                        and ast.AffordabilityAssessmentStatusKey = 1
                    order by
	                        aa.ModifiedDate desc";
        }
    }
}