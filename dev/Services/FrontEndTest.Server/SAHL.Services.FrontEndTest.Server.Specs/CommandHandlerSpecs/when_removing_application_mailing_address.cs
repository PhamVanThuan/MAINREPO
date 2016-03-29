using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_application_mailing_address : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveApplicationMailingAddressCommand command;
        private static RemoveApplicationMailingAddressCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int applicationNumber;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            applicationNumber = 123;
            command = new RemoveApplicationMailingAddressCommand(applicationNumber);
            commandHandler = new RemoveApplicationMailingAddressCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_remove_application_mailing_address = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveApplicationMailingAddress(applicationNumber));
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}