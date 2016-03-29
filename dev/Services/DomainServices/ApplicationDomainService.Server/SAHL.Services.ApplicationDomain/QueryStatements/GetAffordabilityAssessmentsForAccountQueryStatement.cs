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
    public class GetAffordabilityAssessmentsForAccountQueryStatement :
                                        IServiceQuerySqlStatement<GetAffordabilityAssessmentsForAccountQuery, AffordabilityAssessmentSummaryModel>
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
	                    [2AM].[dbo].Offer o 
                    join
	                    [2AM].[dbo].[AffordabilityAssessment] aa
                    on
	                    aa.GenericKey = o.OfferKey
                    join
	                    [2AM].[dbo].AffordabilityAssessmentStatus ast
                    on
	                    ast.AffordabilityAssessmentStatusKey = aa.AffordabilityAssessmentStatusKey
                    where
	                    o.ReservedAccountKey = @AccountKey
                    and 
	                    aa.GenericKeyTypeKey = 2
                    order by
	                        aa.ModifiedDate desc";
        }
    }
}
