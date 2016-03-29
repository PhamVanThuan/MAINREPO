using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_an_application_role_from_an_application : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveApplicationRoleFromApplicationCommand command;
        private static RemoveApplicationRoleFromApplicationCommandHandler commandHandler;
        private static int offerRoleKey;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            testDataManager = An<ITestDataManager>();
            offerRoleKey = 123;
            command = new RemoveApplicationRoleFromApplicationCommand(offerRoleKey);
            commandHandler = new RemoveApplicationRoleFromApplicationCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_remove_the_correct_offerRole_entry = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveApplicationRole(offerRoleKey));
        };
    }
}