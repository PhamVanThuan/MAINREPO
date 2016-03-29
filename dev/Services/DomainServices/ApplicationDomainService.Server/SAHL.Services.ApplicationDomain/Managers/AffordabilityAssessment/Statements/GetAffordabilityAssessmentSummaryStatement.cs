using SAHL.Core.Data;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class GetAffordabilityAssessmentSummaryStatement : ISqlStatement<AffordabilityAssessmentSummaryModel>
    {
        public int AffordabilityAssessmentKey { get; protected set; }

        public GetAffordabilityAssessmentSummaryStatement(int affordabilityAssessmentKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        public string GetStatement()
        {
            var sql = @"select
	                        aa.AffordabilityAssessmentKey as 'Key',
                            aa.GenericKey,
	                        [dbo].[fGetAffordabilityAssessmentContributors] (aa.AffordabilityAssessmentKey,1,0) as 'ClientDetail',
                            aa.AffordabilityAssessmentStatusKey,
	                        ast.Description as 'AffordabilityAssessmentStatus',
	                        ad.ADUserName as 'UserLastAmended',
	                        aa.ModifiedDate as 'DateLastAmended',
	                        aa.NumberOfHouseholdDependants as 'HouseholdDependants',
	                        aa.NumberOfContributingApplicants as 'ContributingApplicants',
                            asf.AffordabilityAssessmentStressFactorKey as 'StressFactorKey',
                            asf.StressFactorPercentage as 'StressFactorPercentageDisplay',
	                        asf.PercentageIncreaseOnRepayments,
	                        aa.MinimumMonthlyFixedExpenses,
	                        aa.ConfirmedDate as 'DateConfirmed'
                        from
	                        [2AM].[dbo].[AffordabilityAssessment] aa
                        join
	                        [2AM].[dbo].[AffordabilityAssessmentStressFactor] asf
                        on
	                        asf.AffordabilityAssessmentStressFactorKey = aa.AffordabilityAssessmentStressFactorKey
                        join
	                        [2AM].[dbo].[AffordabilityAssessmentStatus] ast
                        on
	                        ast.AffordabilityAssessmentStatusKey = aa.AffordabilityAssessmentStatusKey
                        join
	                        [2AM].[dbo].[ADUser] ad
                        on
	                        ad.ADUserKey = aa.ModifiedByUserId
                        where
	                        aa.AffordabilityAssessmentKey = @AffordabilityAssessmentKey
	                        and aa.GenericKeyTypeKey = 2";
            return sql;
        }
    }
}