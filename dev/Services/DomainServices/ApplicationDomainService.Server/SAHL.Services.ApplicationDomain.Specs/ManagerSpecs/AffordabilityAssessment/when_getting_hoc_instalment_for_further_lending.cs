using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_getting_hoc_instalment_for_further_lending : WithFakes
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
            affordabilityAssessmentManager.GetHocInstalmentForFurtherLendingApplication(Param.IsAny<int>());
        };

        private It should_call_the_affordability_assessment_data_manager = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.GetHocInstalmentForFurtherLendingApplication(0));
        };
    }
}