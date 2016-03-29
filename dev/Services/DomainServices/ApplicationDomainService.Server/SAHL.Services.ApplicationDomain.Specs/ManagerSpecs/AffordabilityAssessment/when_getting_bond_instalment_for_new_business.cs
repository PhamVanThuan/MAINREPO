using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_getting_bond_instalment_for_new_business : WithFakes
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
            affordabilityAssessmentManager.GetBondInstalmentForNewBusinessApplication(Param.IsAny<int>());
        };

        private It should_call_the_affordability_assessment_data_manager = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.GetBondInstalmentForNewBusinessApplication(0));
        };
    }
}