using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ProcessInvoicePaymentTransactionReversal
{
    public class when_retrieving_financial_transaction_key_fails : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static IFinanceDataManager financeDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler handler;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand command;
        private static int accountKey;
        private static GetThirdPartyInvoiceQueryResult thirdPartyInvoice;

        private Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            loanTransactionManager = An<ILoanTransactionManager>();
            financeDataManager = An<IFinanceDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            metadata = An<IServiceRequestMetadata>();

            handler = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler(loanTransactionManager, financeDataManager, eventRaiser, serviceQueryRouter);
            command = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(1408);

            accountKey = 10008;

            financeDataManager.WhenToldTo(x => x.GetFinancialTransactionKeyByReference(Param.IsAny<string>())).Return(0);
            thirdPartyInvoice = new GetThirdPartyInvoiceQueryResult
            {
                InvoiceDate = DateTime.Now,
                AccountKey = accountKey,
                AmountExcludingVAT = 1000,
                CapitaliseInvoice = true,
                InvoiceStatusKey = (int)InvoiceStatus.Approved,
                InvoiceNumber = "108",
                ReceivedDate = DateTime.Now,
                ReceivedFromEmailAddress = "johnsnow@blackattorneys.com",
                SahlReference = "SAHL 14/08/88",
                ThirdPartyId = new Guid(),
                ThirdPartyInvoiceKey = 1405,
                ThirdPartyRegisteredName = "John Snow",
                TotalAmountIncludingVAT = 1140,
                VATAmount = 140
            };

            var thirdPartyInvoiceList = new List<GetThirdPartyInvoiceQueryResult> { thirdPartyInvoice };
            var results = new ServiceQueryResult<GetThirdPartyInvoiceQueryResult>(thirdPartyInvoiceList);
            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param.IsAny<GetThirdPartyInvoiceQuery>())).Return<GetThirdPartyInvoiceQuery>(y =>
            {
                y.Result = results;
                return SystemMessageCollection.Empty();
            });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_get_the_third_party_invoice_using_the_third_party_key = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetThirdPartyInvoiceQuery>.Matches(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey)));
        };

        private It should_get_the_accounts_variable_loan_financialTransactionKey = () =>
        {
            financeDataManager.WasToldTo(x => x.GetFinancialTransactionKeyByReference(thirdPartyInvoice.SahlReference));
        };

        private It should_return_warning_messages = () =>
        {
            messages.HasWarnings.ShouldBeTrue();
            messages.AllMessages.First().Message.ShouldEqual("Failed to retrieve the financial transaction key");
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_not_post_the_transaction = () =>
        {
            loanTransactionManager.WasNotToldTo(x =>
                x.PostTransaction(Param.IsAny<int>(), Param.IsAny<LoanTransactionTypeEnum>(),
               Param.IsAny<decimal>(), Param.IsAny<DateTime>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_not_raise_an_reversal_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<TransactionProcessedForThirdPartyInvoicePaymentReversalEvent>(), thirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata));
        };
    }
}