using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetActiveAffordabilityAssessmentsByGenericKeyStatement : ISqlStatement<AffordabilityAssessmentDataModel>
    {
        public int GenericKey { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public GetActiveAffordabilityAssessmentsByGenericKeyStatement(int genericKey, int genericKeyTypeKey)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
        }

        public string GetStatement()
        {
            return @"
SELECT *
FROM [2AM].[dbo].[AffordabilityAssessment]
WHERE [GenericKey] = @GenericKey
AND [GenericKeyTypeKey] = @GenericKeyTypeKey
AND [GeneralStatusKey] = 1";
        }
    }
}