using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager.Statements
{
    public class GetAffordabilityAssessmentByKeyStatement : ISqlStatement<AffordabilityAssessmentDataModel>
    {
        public int AffordabilityAssessmentKey { get; protected set; }

        public GetAffordabilityAssessmentByKeyStatement(int affordabilityAssessmentKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        public string GetStatement()
        {
            string query = @"select * from [2AM].dbo.AffordabilityAssessment where AffordabilityAssessmentKey = @AffordabilityAssessmentKey";
            return query;
        }
    }
}