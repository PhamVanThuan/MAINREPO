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
    public class when_processing_a_transaction : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static IFinanceDataManager financeDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler handler;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand command;
        private static int financialTransactionKey, accountKey;
        private static GetThirdPartyInvoiceQueryResult thirdPartyInvoice;

        Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            loanTransactionManager = An<ILoanTransactionManager>();
            financeDataManager = An<IFinanceDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            metadata = An<IServiceRequestMetadata>();

            handler = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler(loanTransactionManager, financeDataManager, eventRaiser, serviceQueryRouter);
            command = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(1408);

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

            loanTransactionManager.WhenToldTo(x =>
                    x.PostTransaction(financialTransactionKey, LoanTransactionTypeEnum.CapitalisedLegalFeeReversalTransaction, (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, 
                    Param.IsAny<DateTime>(), thirdPartyInvoice.SahlReference, metadata.UserName)
                ).Return(SystemMessageCollection.Empty());

        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        It should_get_the_third_party_invoice_using_the_third_party_key = () =>
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

        It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<TransactionProcessedForThirdPartyInvoicePaymentReversalEvent>.Matches(y =>
                y.AccountKey == thirdPartyInvoice.AccountKey &&
                y.Amount == (decimal)thirdPartyInvoice.TotalAmountIncludingVAT &&
                y.ThirdPartyInvoiceKey == thirdPartyInvoice.ThirdPartyInvoiceKey &&
                y.FinancialTransactionKey == financialTransactionKey
                ), thirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        It should_not_return_any_messages_at_all = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };


    }
}
