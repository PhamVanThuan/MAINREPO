using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_creating_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        private static int modifiedByUserId;
        private static int stressFactorKey;
        private static string defaultStressFactorPercentage;

        private static AffordabilityAssessmentModel affordabilityAssessment;

        private Establish context = () =>
        {
            modifiedByUserId = 999;
            stressFactorKey = 1;
            defaultStressFactorPercentage = "0.5%";
            affordabilityAssessment = new AffordabilityAssessmentModel();
            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();

            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentStressFactorKeyByStressFactorPercentage(defaultStressFactorPercentage))
                                              .Return(stressFactorKey);

            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.CreateAffordabilityAssessment(affordabilityAssessment, modifiedByUserId);
        };

        private It should_query_for_the_default_affordability_assessment_stress_factor = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.GetAffordabilityAssessmentStressFactorKeyByStressFactorPercentage(defaultStressFactorPercentage));
        };

        private It should_tell_the_affordability_assessment_data_manager_to_save_a_datamodel = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.InsertAffordabilityAssessment(Param.IsAny<AffordabilityAssessmentDataModel>()));
        };
    }
}