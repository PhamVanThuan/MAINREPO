using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_a_file_given_a_path : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveFileByPathCommandhandler commandHandler;
        private static RemoveFileByPathCommand command;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = An<ISystemMessageCollection>();
            metadata = An<IServiceRequestMetadata>();
            command = new RemoveFileByPathCommand("C:/temp/123.pdf");
            commandHandler = new RemoveFileByPathCommandhandler();
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_error_messages_if_the_path_is_invalid = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}