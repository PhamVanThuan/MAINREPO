using System;
using DomainService2.Specs.DomainObjects;
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
    public class When_there_are_valid_transactions_in_the_last_24_hours : CanRollbackReadvanceCorrectionTransactionHandlerBase
    {
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            loanTransactionRepository = An<ILoanTransactionRepository>();
            IApplication app = An<IApplication>();
            IAccount account = An<IAccount>();
            IFinancialTransaction correctionTransaction = An<IFinancialTransaction>();
            IFinancialTransaction financialTransaction = An<IFinancialTransaction>();
            IReadOnlyEventList<IFinancialTransaction> listofCorrections = new StubReadOnlyEventList<IFinancialTransaction>(new IFinancialTransaction[] { correctionTransaction });
            IReadOnlyEventList<IFinancialTransaction> listofTransactions = new StubReadOnlyEventList<IFinancialTransaction>(new IFinancialTransaction[] { financialTransaction });

            messages = new DomainMessageCollection();

            app.WhenToldTo(x => x.Account).Return(account);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).
                Return(app);

            loanTransactionRepository.WhenToldTo(x => x.GetLoanTransactionsByTransactionTypeAndAccountKey(Param.Is<SAHL.Common.Globals.TransactionTypes>(SAHL.Common.Globals.TransactionTypes.ReadvanceCorrection), Param.IsAny<int>())).
                Return(listofCorrections);

            loanTransactionRepository.WhenToldTo(x => x.GetLoanTransactionsByTransactionTypeAndAccountKey(Param.Is<SAHL.Common.Globals.TransactionTypes>(SAHL.Common.Globals.TransactionTypes.Readvance), Param.IsAny<int>())).
                Return(listofTransactions);

            // add a valid transaction to the list
            DateTime tranTime = DateTime.Now;
            double amount = 5000.0;
            correctionTransaction.WhenToldTo(x => x.InsertDate).Return(tranTime.AddHours(-1));
            financialTransaction.WhenToldTo(x => x.InsertDate).Return(tranTime.AddHours(-2));
            financialTransaction.WhenToldTo(x => x.IsRolledBack).Return(true);
            financialTransaction.WhenToldTo(x => x.Amount).Return(amount);
            correctionTransaction.WhenToldTo(x => x.Amount).Return(amount * -1);

            command = new CanRollbackReadvanceCorrectionTransactionCommand(Param.IsAny<int>());
            handler = new CanRollbackReadvanceCorrectionTransactionCommandHandler(applicationRepository, loanTransactionRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_have_an_empty_messagecollection = () =>
        {
            messages.Count.ShouldEqual<int>(0);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}