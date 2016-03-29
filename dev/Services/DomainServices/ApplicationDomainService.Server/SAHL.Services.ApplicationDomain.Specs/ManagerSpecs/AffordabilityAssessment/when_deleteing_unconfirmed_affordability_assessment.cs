using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_deleteing_unconfirmed_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        private Establish context = () =>
        {
            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();
            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.DeleteUnconfirmedAffordabilityAssessment(Param.IsAny<int>());
        };

        private It should_call_the_affordability_assessment_data_manager_delete = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.DeleteUnconfirmedAffordabilityAssessment(Param.IsAny<int>()));
        };
    }
}