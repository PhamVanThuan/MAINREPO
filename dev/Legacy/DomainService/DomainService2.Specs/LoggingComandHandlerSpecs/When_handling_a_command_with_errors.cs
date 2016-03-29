namespace DomainService2.Specs.LoggingCommandHandlerSpecs
{
    using System;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Logging;
    using SAHL.X2.Framework;
    using System.Collections.Generic;

    [Subject(typeof(LoggingCommandHandler<>), "Handle Command")]
    public class When_handling_a_command_with_errors : WithFakes
    {
        static LoggingCommandHandler<DummyCommand> loggingCommandHandler;
        static IHandlesDomainServiceCommand<DummyCommand> innerHandler;
        static ILogger logger;
        static DummyCommand command;
        static IDomainMessageCollection messages;
		static Exception ex;

        Establish context = () =>
        {
            // create a messages collection
            messages = An<IDomainMessageCollection>();
            messages.WhenToldTo(x => x.HasErrorMessages).Return(true);

            command = new DummyCommand(false);

            // mock the constructor arguments
            // an inner handler
            innerHandler = new DummyCommandHandlerThatThrowsException();


            // a logger
            logger = An<ILogger>();

            // create an instance of the system under test
            loggingCommandHandler = new DomainService2.LoggingCommandHandler<DummyCommand>(innerHandler, logger);
        };

        Because of = () =>
        {
			ex = Catch.Exception(() => { loggingCommandHandler.Handle(messages, command); });
        };

		It should_catch_exception = () =>
		{
			ex.ShouldNotBeNull();
		};

        It should_log_onenter = () =>
        {
            logger.WasToldTo(x => x.LogOnEnterMethod(Param<string>.IsAnything, Param<Dictionary<string, object>>.IsAnything)).OnlyOnce();
        };

        It should_log_exception = () =>
        {
            logger.WasToldTo(x => x.LogOnMethodException(Param<string>.IsAnything, Param<Exception>.IsAnything, Param<Dictionary<string, object>>.IsAnything)).OnlyOnce();
        };

        It should_log_onexit = () =>
        {
            logger.WasToldTo(x => x.LogOnExitMethod(Param<string>.IsAnything, Param<Dictionary<string, object>>.IsAnything)).OnlyOnce();
        };
    }
}