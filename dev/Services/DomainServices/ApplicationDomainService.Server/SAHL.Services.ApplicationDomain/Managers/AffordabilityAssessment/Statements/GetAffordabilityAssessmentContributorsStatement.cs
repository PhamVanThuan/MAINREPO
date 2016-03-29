using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetAffordabilityAssessmentContributorsStatement : ISqlStatement<int>
    {
        public int AffordabilityAssessmentKey { get; protected set; }

        public GetAffordabilityAssessmentContributorsStatement(int affordabilityAssessmentKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        public string GetStatement()
        {
            return @"select
	                    LegalEntityKey
                    from
	                    [2AM].dbo.AffordabilityAssessmentLegalEntity
                    where
	                    AffordabilityAssessmentKey = @AffordabilityAssessmentKey";
        }
    }
}