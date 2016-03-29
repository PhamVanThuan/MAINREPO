using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetLatestConfirmedAffordabilityAssessmentByGenericKeyStatement : ISqlStatement<AffordabilityAssessmentDataModel>
    {
        public int GenericKey { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public GetLatestConfirmedAffordabilityAssessmentByGenericKeyStatement(int genericKey, int genericKeyTypeKey)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
        }

        public string GetStatement()
        {
            return @"SELECT TOP 1 * 
                     FROM [2AM].[dbo].[AffordabilityAssessment] 
                     WHERE [GenericKey] = @GenericKey 
                        AND [GenericKeyTypeKey] = @GenericKeyTypeKey 
                        AND [AffordabilityAssessmentStatusKey] = 2 
                     ORDER BY [AffordabilityAssessmentKey] DESC";
        }
    }
}