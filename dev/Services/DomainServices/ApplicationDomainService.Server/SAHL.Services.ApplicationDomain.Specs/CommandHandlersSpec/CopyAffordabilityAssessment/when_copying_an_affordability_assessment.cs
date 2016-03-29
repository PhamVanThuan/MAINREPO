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
    public class when_copying_an_affordability_assessment : WithCoreFakes
    {
        private static CopyAffordabilityAssessmentCommand command;
        private static CopyAffordabilityAssessmentCommandHandler handler;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IADUserManager adUserManager;
        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private Establish context = () =>
        {
            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();
            adUserManager = An<IADUserManager>();
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(Param.IsAny<string>())).Return(99);
            affordabilityAssessmentModel = new AffordabilityAssessmentModel();

            command = new CopyAffordabilityAssessmentCommand(affordabilityAssessmentModel);
            handler = new CopyAffordabilityAssessmentCommandHandler(adUserManager, affordabilityAssessmentManager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_copy_the_affordability_assessment = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.CopyAndArchiveAffordabilityAssessmentWithNewAffordabilityAssessmentItems(affordabilityAssessmentModel, 99));
        };
    }
}