using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class ConfirmAffordabilityAssessmentStatement : ISqlStatement<AffordabilityAssessmentDataModel>
    {
        public ConfirmAffordabilityAssessmentStatement(int affordabilityAssessmentKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        public int AffordabilityAssessmentKey { get; protected set; }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [dbo].[AffordabilityAssessment]
                    SET [AffordabilityAssessmentStatusKey] = {0},
                        [ConfirmedDate] = GetDate()
                    WHERE [AffordabilityAssessmentKey] = @AffordabilityAssessmentKey", (int)AffordabilityAssessmentStatus.Confirmed);
        }
    }
}