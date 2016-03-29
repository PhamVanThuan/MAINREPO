using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresOpenApplication
{
    class when_open_application_does_not_exist : WithFakes
    {
        static IApplicationDataManager applicationDataManager;
        static ISystemMessageCollection systemMessages;
        static RequiresOpenApplicationCheckHandler handler;
        static IRequiresOpenApplication command;

        Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            command = An<IRequiresOpenApplication>();
            handler = new RequiresOpenApplicationCheckHandler(applicationDataManager);
            applicationDataManager.WhenToldTo(x => x.IsApplicationOpen(command.ApplicationNumber)).Return(false);
        };

        Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(command);
        };

        It should_check_if_open_application_exists = () =>
        {
            applicationDataManager.WasToldTo(x => x.IsApplicationOpen(command.ApplicationNumber));
        };

        It should_return_error_messages = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldEqual("No open application could be found against your application number.");
        };
    }
}
