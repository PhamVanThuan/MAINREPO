using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_linking_a_legalentity_to_an_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        private static int affordabilityAssessmentKey;
        private static int legalEntityKey;

        private Establish context = () =>
        {
            affordabilityAssessmentKey = 999999;
            legalEntityKey = 111111;

            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();
            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.LinkLegalEntityToAffordabilityAssessment(affordabilityAssessmentKey, legalEntityKey);
        };

        private It should_tell_the_affordability_assessment_data_manager_to_insert_the_link = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.InsertAffordabilityAssessmentLegalEntity(Arg.Is<AffordabilityAssessmentLegalEntityDataModel>(y =>
                   y.AffordabilityAssessmentKey == affordabilityAssessmentKey &&
                   y.LegalEntityKey == legalEntityKey)));
        };
    }
}