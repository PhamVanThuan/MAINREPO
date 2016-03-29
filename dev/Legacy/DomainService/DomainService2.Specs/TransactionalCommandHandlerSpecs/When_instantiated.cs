namespace DomainService2.Specs.TransactionalCommandHandlerSpecs
{
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.DataAccess;

    [Subject(typeof(DomainService2.TransactionalCommandHandler<>), "On Construction")]
    public class When_instantiated : WithFakes
    {
        static DomainService2.TransactionalCommandHandler<DummyCommand> transactionalCommandHandler;
        static IHandlesDomainServiceCommand<DummyCommand> innerHandler;
        static ITransactionManager transactionManager;

        Establish context = () =>
        {
            // mock the constructor arguments
            innerHandler = An<IHandlesDomainServiceCommand<DummyCommand>>();
            transactionManager = An<ITransactionManager>();
        };

        Because of = () =>
        {
            // create an instance of the system under test
            transactionalCommandHandler = new DomainService2.TransactionalCommandHandler<DummyCommand>(innerHandler, transactionManager);
        };

        It should_set_the_innerhandler_property = () =>
        {
            transactionalCommandHandler.InnerHandler.ShouldBeTheSameAs(innerHandler);
        };
    }
}