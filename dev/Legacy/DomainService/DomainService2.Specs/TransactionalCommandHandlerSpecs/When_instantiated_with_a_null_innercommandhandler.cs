namespace DomainService2.Specs.TransactionalCommandHandlerSpecs
{
    using System;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.DataAccess;

    [Subject(typeof(DomainService2.TransactionalCommandHandler<>), "On Construction")]
    public class When_instantiated_with_a_null_innercommandhandler : WithFakes
    {
        static DomainService2.TransactionalCommandHandler<DummyCommand> transactionalCommandHandler;
        static IHandlesDomainServiceCommand<DummyCommand> innerHandler;
        static ITransactionManager transactionManager;

        static Exception exception;

        Establish context = () =>
        {
            // mock the constructor arguments
            innerHandler = null;
            transactionManager = An<ITransactionManager>();
        };

        Because of = () =>
        {
            // create an instance of the system under test
            exception = Catch.Exception(() => transactionalCommandHandler = new DomainService2.TransactionalCommandHandler<DummyCommand>(innerHandler, transactionManager));
        };

        It should_thrown_a_ArgumentNullException = () =>
        {
            exception.ShouldBeOfType<ArgumentNullException>();
        };

        It should_set_the_exception_parametername_property = () =>
        {
            ArgumentNullException argEx = exception as ArgumentNullException;
            argEx.ParamName.ShouldEqual<string>(Strings.ArgInnerHandler);
        };
    }
}