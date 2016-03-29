using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetHistoricalAffordabilityAssessmentsForApplicationQueryStatement : 
                                        IServiceQuerySqlStatement<GetHistoricalAffordabilityAssessmentsForApplicationQuery, AffordabilityAssessmentSummaryModel>
    {
        public string GetStatement()
        {
            return @"select
                        aa.GenericKey,
	                    aa.AffordabilityAssessmentKey as 'Key',
						aa.ModifiedDate as 'DateLastAmended',
						[dbo].[fGetAffordabilityAssessmentContributors] (aa.AffordabilityAssessmentKey,1,0) as 'ClientDetail',
						ast.[Description] as 'AssessmentStatus',
						aa.ConfirmedDate as 'DateConfirmed',
                        aa.GeneralStatusKey
                    from
	                    [2AM].[dbo].[AffordabilityAssessment] aa
                    join
	                    [2AM].[dbo].AffordabilityAssessmentStatus ast
                    on
	                    ast.AffordabilityAssessmentStatusKey = aa.AffordabilityAssessmentStatusKey
                    where
	                    aa.GenericKey = @ApplicationKey
	                    and aa.GenericKeyTypeKey = 2
	                    and aa.GeneralStatusKey = 2
                        and aa.AffordabilityAssessmentStatusKey = 2
                    order by
	                        aa.ModifiedDate desc";
        }
    }
}
