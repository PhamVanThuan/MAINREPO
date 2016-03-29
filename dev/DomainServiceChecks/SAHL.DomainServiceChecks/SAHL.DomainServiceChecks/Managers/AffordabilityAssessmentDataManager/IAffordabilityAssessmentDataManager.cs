using SAHL.Core.Data.Models._2AM;

namespace SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager
{
    public interface IAffordabilityAssessmentDataManager
    {
        AffordabilityAssessmentDataModel GetAffordabilityAssessmentByKey(int affordabilityAssessmentKey);
    }
}