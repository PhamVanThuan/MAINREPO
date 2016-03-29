using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetAffordabilityAssessmentItemsStatement : ISqlStatement<AffordabilityAssessmentItemDataModel>
    {
        public int AffordabilityAssessmentKey { get; protected set; }

        public GetAffordabilityAssessmentItemsStatement(int affordabilityAssessmentKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        public string GetStatement()
        {
            return @"SELECT * FROM [2AM].[dbo].[AffordabilityAssessmentItem] WHERE AffordabilityAssessmentKey = @AffordabilityAssessmentKey";
        }
    }
}