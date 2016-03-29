namespace DomainService2.Specs.TransactionalCommandHandlerSpecs
{
    using System;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.DataAccess;

    [Subject(typeof(DomainService2.TransactionalCommandHandler<>), "Handle Command")]
    public class When_handling_a_command_with_domainwarningsmessages_and_ignorewarnings_is_true : WithFakes
    {
        static DomainService2.TransactionalCommandHandler<DummyCommand> transactionalCommandHandler;
        static IHandlesDomainServiceCommand<DummyCommand> innerHandler;
        static ITransactionManager transactionManager;
        static DummyCommand command;
        static IDomainMessageCollection messages;

        static Exception exception;

        Establish context = () =>
        {
            // create a messages collection
            messages = An<IDomainMessageCollection>();
            messages.WhenToldTo(x => x.HasErrorMessages).Return(false);
            messages.WhenToldTo(x => x.HasWarningMessages).Return(true);

            command = new DummyCommand(true);

            // mock the constructor arguments
            innerHandler = An<IHandlesDomainServiceCommand<DummyCommand>>();
            transactionManager = An<ITransactionManager>();

            // create an instance of the system under test
            transactionalCommandHandler = new DomainService2.TransactionalCommandHandler<DummyCommand>(innerHandler, transactionManager);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => transactionalCommandHandler.Handle(messages, command));
        };

        It should_start_a_transaction = () =>
        {
            transactionManager.WasToldTo(x => x.BeginTransaction()).OnlyOnce();
        };

        It should_ask_the_innerhandler_to_handle_the_command = () =>
        {
            innerHandler.WasToldTo(x => x.Handle(messages, command)).OnlyOnce();
        };

        It should_not_rollback_the_transaction = () =>
        {
            transactionManager.WasNotToldTo(x => x.RollbackTransaction());
        };

        It should_not_throw_a_domainmessageexception = () =>
        {
            exception.ShouldBeNull();
        };
    }
}