using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_an_attorney : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveAttorneyCommand command;
        private static RemoveAttorneyCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int attorneyKey;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            attorneyKey = 123;
            command = new RemoveAttorneyCommand(attorneyKey);
            commandHandler = new RemoveAttorneyCommandHandler(testDataManager);
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

        private It should_remove_the_correct_attorney = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveAttorney(attorneyKey));
        };
    }
}