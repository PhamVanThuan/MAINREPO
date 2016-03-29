using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresOpenLatestApplicationInformation
{
    public class when_latest_application_information_is_not_open : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static ISystemMessageCollection systemMessages;
        private static RequiresOpenLatestApplicationInformationHandler handler;
        private static IRequiresOpenLatestApplicationInformation command;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            command = An<IRequiresOpenLatestApplicationInformation>();
            handler = new RequiresOpenLatestApplicationInformationHandler(applicationDataManager);
            applicationDataManager.WhenToldTo(x => x.IsLatestApplicationInformationOpen(command.ApplicationNumber)).Return(false);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(command);
        };

        private It should_check_for_an_open_application_in_our_system = () =>
        {
            applicationDataManager.WasToldTo(x => x.IsLatestApplicationInformationOpen(command.ApplicationNumber));
        };

        private It should_not_return_any_error_messages = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldEqual("The latest application information for your application is not open.");
        };
    }
}