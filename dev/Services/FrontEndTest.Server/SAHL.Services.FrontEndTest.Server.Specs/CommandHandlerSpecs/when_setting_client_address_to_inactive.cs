using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_setting_client_address_to_inactive : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static SetClientAddressToInactiveCommand command;
        private static SetClientAddressToInactiveCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int clientKey;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            messages = An<ISystemMessageCollection>();
            clientKey = 5;
            command = new SetClientAddressToInactiveCommand(clientKey);
            commandHandler = new SetClientAddressToInactiveCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_set_the_client_address_to_inactive = () =>
        {
            testDataManager.WasToldTo(x => x.SetClientAddressToInactive(command.ClientAddressKey));
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}