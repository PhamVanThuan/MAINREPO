using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.RollbackTransactionCommandHandlerSpecs
{
    [Subject(typeof(RollbackTransactionCommandHandler))]
    public class When_rollback_is_successful : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static RollbackTransactionCommand command;
        protected static RollbackTransactionCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static string msg = string.Empty;
        protected static ICommonRepository commonRepository;

        // Arrange
        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            MockRepository mocks = new MockRepository();
            debtCounsellingRepository = mocks.StrictMock<IDebtCounsellingRepository>();

            debtCounsellingRepository.WhenToldTo(x => x.RollbackTransaction(Param<int>.IsAnything)).Return(true);

            messages = new DomainMessageCollection();
            command = new RollbackTransactionCommand(Param<int>.IsAnything);
            handler = new RollbackTransactionCommandHandler(debtCounsellingRepository, commonRepository);

            mocks.ReplayAll();
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };

        // Assert
        It should_return_no_error_message = () =>
        {
            messages.Count.Equals(0);
        };
    }
}