using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.CanRollbackReadvanceCorrectionTransactionHandlerSpecs
{
    [Subject(typeof(CanRollbackReadvanceCorrectionTransactionCommandHandler))]
    public class When_there_are_no_transactions : CanRollbackReadvanceCorrectionTransactionHandlerBase
    {
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            loanTransactionRepository = An<ILoanTransactionRepository>();
            IReadOnlyEventList<IFinancialTransaction> listofCorrections = An<ReadOnlyEventList<IFinancialTransaction>>();
            IReadOnlyEventList<IFinancialTransaction> listofTransactions = An<ReadOnlyEventList<IFinancialTransaction>>();
            IApplication app = An<IApplication>();
            IAccount account = An<IAccount>();
            messages = new DomainMessageCollection();

            app.WhenToldTo(x => x.Account).Return(account);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).
                Return(app);

            loanTransactionRepository.WhenToldTo(x => x.GetLoanTransactionsByTransactionTypeAndAccountKey(Param.Is<SAHL.Common.Globals.TransactionTypes>(SAHL.Common.Globals.TransactionTypes.ReadvanceCorrection), Param.IsAny<int>())).
                Return(listofCorrections);

            loanTransactionRepository.WhenToldTo(x => x.GetLoanTransactionsByTransactionTypeAndAccountKey(Param.Is<SAHL.Common.Globals.TransactionTypes>(SAHL.Common.Globals.TransactionTypes.Readvance), Param.IsAny<int>())).
                Return(listofTransactions);

            command = new CanRollbackReadvanceCorrectionTransactionCommand(0);
            handler = new CanRollbackReadvanceCorrectionTransactionCommandHandler(applicationRepository, loanTransactionRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_have_added_an_error_to_the_messagecollection = () =>
        {
            messages.HasErrorMessages.ShouldBeTrue();
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}