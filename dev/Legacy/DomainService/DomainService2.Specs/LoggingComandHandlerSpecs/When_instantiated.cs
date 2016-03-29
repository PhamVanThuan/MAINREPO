namespace DomainService2.Specs.LoggingCommandHandlerSpecs
{
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.Logging;

    [Subject(typeof(DomainService2.LoggingCommandHandler<>), "On Construction")]
    public class When_instantiated : WithFakes
    {
        static DomainService2.LoggingCommandHandler<DummyCommand> loggingCommandHandler;
        static IHandlesDomainServiceCommand<DummyCommand> innerHandler;
        static ILogger logger;

        Establish context = () =>
        {
            // mock the constructor arguments
            innerHandler = An<IHandlesDomainServiceCommand<DummyCommand>>();
            logger = An<ILogger>();
        };

        Because of = () =>
        {
            // create an instance of the system under test
            loggingCommandHandler = new DomainService2.LoggingCommandHandler<DummyCommand>(innerHandler, logger);
        };

        It should_set_the_innerhandler_property = () =>
        {
            loggingCommandHandler.InnerHandler.ShouldBeTheSameAs(innerHandler);
        };
    }
}