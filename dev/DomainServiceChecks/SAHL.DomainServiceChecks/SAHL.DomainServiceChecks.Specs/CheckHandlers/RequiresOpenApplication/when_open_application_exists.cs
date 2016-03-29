using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresOpenApplication
{
    public class when_open_application_exists : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static ISystemMessageCollection systemMessages;
        private static RequiresOpenApplicationCheckHandler handler;
        private static IRequiresOpenApplication command;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            command = An<IRequiresOpenApplication>();
            handler = new RequiresOpenApplicationCheckHandler(applicationDataManager);
            applicationDataManager.WhenToldTo(x => x.IsApplicationOpen(command.ApplicationNumber)).Return(true);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(command);
        };

        private It should_check_if_open_application_exists = () =>
        {
            applicationDataManager.WasToldTo(x => x.IsApplicationOpen(command.ApplicationNumber));
        };

        private It should_not_return_any_error_messages = () =>
        {
            systemMessages.AllMessages.Count().ShouldEqual(0);
        };
    }
}