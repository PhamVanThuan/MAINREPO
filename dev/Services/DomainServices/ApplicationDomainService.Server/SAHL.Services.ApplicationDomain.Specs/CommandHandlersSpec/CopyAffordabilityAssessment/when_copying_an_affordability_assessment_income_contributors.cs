using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers.Internal;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.CopyAffordabilityAssessment
{
    public class when_copying_an_affordability_assessment_income_contributors : WithCoreFakes
    {
        private static CopyAffordabilityAssessmentIncomeContributorsCommand command;
        private static CopyAffordabilityAssessmentIncomeContributorsCommandHandler handler;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IADUserManager adUserManager;
        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private Establish context = () =>
        {
            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();
            adUserManager = An<IADUserManager>();
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(Param.IsAny<string>())).Return(99);
            affordabilityAssessmentModel = new AffordabilityAssessmentModel();

            command = new CopyAffordabilityAssessmentIncomeContributorsCommand(affordabilityAssessmentModel);
            handler = new CopyAffordabilityAssessmentIncomeContributorsCommandHandler(adUserManager, affordabilityAssessmentManager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_copy_the_affordability_assessment = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.CopyAndArchiveAffordabilityAssessmentWithNewIncomeContributors(affordabilityAssessmentModel, 99));
        };
    }
}