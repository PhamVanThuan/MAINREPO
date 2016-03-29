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
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ProcessInvoicePaymentTransactionReversal
{
    public class when_posting_the_transaction_fails : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static IFinanceDataManager financeDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler handler;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand command;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static int financialTransactionKey, accountKey;
        private static GetThirdPartyInvoiceQueryResult thirdPartyInvoice;

        Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            loanTransactionManager = An<ILoanTransactionManager>();
            financeDataManager = An<IFinanceDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            command = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(10008);
            handler = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler(loanTransactionManager, financeDataManager, eventRaiser, serviceQueryRouter);

            metadata = An<IServiceRequestMetadata>();
            messages = SystemMessageCollection.Empty();

            financialTransactionKey = 108;
            accountKey = 10008;
            financeDataManager.WhenToldTo(x => x.GetVariableLoanFinancialServiceKeyByAccount(Param.IsAny<int>())).Return(financialTransactionKey);
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

            financeDataManager.WhenToldTo(x => x.GetFinancialTransactionKeyByReference(thirdPartyInvoice.SahlReference)).Return(financialTransactionKey);

            var errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("An error occured while attempting to post the transaction", SystemMessageSeverityEnum.Error));
            loanTransactionManager.WhenToldTo(x =>
                    x.PostReversalTransaction(financialTransactionKey, metadata.UserName)
                ).Return(errorMessages);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        It should_get_the_third_party_invoice_using_the_third_party_invoice_key = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetThirdPartyInvoiceQuery>.Matches(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey)));
        };

        It should_get_the_financial_transaction_key = () =>
        {
            financeDataManager.WasToldTo(x => x.GetFinancialTransactionKeyByReference(thirdPartyInvoice.SahlReference));
        };

        It should_post_the_reversal_transaction = () =>
        {
            loanTransactionManager.WasToldTo(x => x.PostReversalTransaction(financialTransactionKey, metadata.UserName));
        };

        It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.AllMessages.First().Message.ShouldEqual("An error occured while attempting to post the transaction");
        };

        It should_not_raise_a_capistalised_legal_fee_transaction_posted_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<TransactionProcessedForThirdPartyInvoicePaymentReversalEvent>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}

