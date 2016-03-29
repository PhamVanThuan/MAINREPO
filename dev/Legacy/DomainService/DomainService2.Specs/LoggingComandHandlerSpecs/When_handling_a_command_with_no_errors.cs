namespace DomainService2.Specs.LoggingCommandHandlerSpecs
{
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Logging;
    using System.Collections.Generic;

    [Subject(typeof(DomainService2.LoggingCommandHandler<>), "Handle Command")]
    public class When_handling_a_command_with_no_errors : WithFakes
    {
        static DomainService2.LoggingCommandHandler<DummyCommand> loggingCommandHandler;
        static IHandlesDomainServiceCommand<DummyCommand> innerHandler;
        static ILogger logger;
        static DummyCommand command;
        static IDomainMessageCollection messages;

        Establish context = () =>
        {
            // mock the constructor arguments
            innerHandler = new DummyCommandHandler();

            logger = An<ILogger>();

            // create an instance of the system under test
            loggingCommandHandler = new DomainService2.LoggingCommandHandler<DummyCommand>(innerHandler, logger);

            command = new DummyCommand(false);

            // create a messages collection
            messages = An<IDomainMessageCollection>();
            messages.WhenToldTo(x => x.HasErrorMessages).Return(false);
        };

        Because of = () =>
        {
            loggingCommandHandler.Handle(messages, command);
        };

        It should_log_onenter = () =>
        {
            logger.WasToldTo(x => x.LogOnEnterMethod(Param<string>.IsAnything, Param<Dictionary<string, object>>.IsAnything)).OnlyOnce();
        };

        It should_log_success = () =>
        {
            logger.WasToldTo(x => x.LogOnMethodSuccess(Param<string>.IsAnything, Param<Dictionary<string, object>>.IsAnything)).OnlyOnce();
        };

        It should_log_onexit = () =>
        {
            logger.WasToldTo(x => x.LogOnExitMethod(Param<string>.IsAnything, Param<Dictionary<string, object>>.IsAnything)).OnlyOnce();
        };
    }
}