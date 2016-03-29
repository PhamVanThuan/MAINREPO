using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class ArchiveAffordabilityAssessmentStatement : ISqlStatement<AffordabilityAssessmentDataModel>
    {
        public ArchiveAffordabilityAssessmentStatement(int affordabilityAssessmentKey, int _ADUserKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.AdUserKey = _ADUserKey;
        }

        public int AffordabilityAssessmentKey { get; protected set; }

        public int AdUserKey { get; protected set; }

        public string GetStatement()
        {
            return @"UPDATE [2AM].[dbo].[AffordabilityAssessment]
SET [GeneralStatusKey] = 2,
	[ModifiedByUserId] = @AdUserKey,
	[ModifiedDate] = GetDate()
WHERE [AffordabilityAssessmentKey] = @AffordabilityAssessmentKey";
        }
    }
}