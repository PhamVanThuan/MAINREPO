using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_application_from_open_new_business_application : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveApplicationFromOpenNewBusinessApplicationsCommand command;
        private static RemoveApplicationFromOpenNewBusinessApplicationsCommandHandler commandHandler;
        private static int offerKey;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            offerKey = 123;
            command = new RemoveApplicationFromOpenNewBusinessApplicationsCommand(offerKey);
            commandHandler = new RemoveApplicationFromOpenNewBusinessApplicationsCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_remove_the_application = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveOpenNewBusinessApplicationCommand(offerKey));
        };
    }
}