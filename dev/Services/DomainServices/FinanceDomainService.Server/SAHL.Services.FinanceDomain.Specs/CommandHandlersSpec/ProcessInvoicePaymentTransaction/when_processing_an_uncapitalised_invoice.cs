using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ProcessInvoicePaymentTransaction
{
    public class when_processing_an_uncapitalised_invoice : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static IFinanceDataManager financeDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private static ProcessTransactionsForThirdPartyInvoicePaymentCommandHandler handler;
        private static ProcessTransactionsForThirdPartyInvoicePaymentCommand command;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static int financialServiceKey, accountKey;
        private static GetThirdPartyInvoiceQueryResult thirdPartyInvoice;
        private static IThirdPartyInvoiceDataManager dataManager;

        Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            loanTransactionManager = An<ILoanTransactionManager>();
            financeDataManager = An<IFinanceDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            domainRuleManager = An<IDomainRuleManager<IThirdPartyInvoiceRuleModel>>();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            handler = new ProcessTransactionsForThirdPartyInvoicePaymentCommandHandler(loanTransactionManager, financeDataManager, eventRaiser, serviceQueryRouter,
                domainRuleManager, dataManager);
            command = new ProcessTransactionsForThirdPartyInvoicePaymentCommand(10008);

            metadata = An<IServiceRequestMetadata>();
            messages = SystemMessageCollection.Empty();

            financialServiceKey = 108;
            accountKey = 10008;
            financeDataManager.WhenToldTo(x => x.GetVariableLoanFinancialServiceKeyByAccount(Param.IsAny<int>())).Return(financialServiceKey);
            thirdPartyInvoice = new GetThirdPartyInvoiceQueryResult
            {
                InvoiceDate = DateTime.Now,
                AccountKey = accountKey,
                AmountExcludingVAT = 1000,
                CapitaliseInvoice = false,
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

            loanTransactionManager.WhenToldTo(x =>
                    x.PostTransaction(financialServiceKey, LoanTransactionTypeEnum.CapitalisedLegalFeeTransaction, (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, Param.IsAny<DateTime>(), thirdPartyInvoice.SahlReference, metadata.UserName)
                ).Return(SystemMessageCollection.Empty());
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        It should_get_the_third_party_invoice_using_the_third_party_invoice_key = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetThirdPartyInvoiceQuery>.Matches(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey)));
        };

        It should_get_the_accounts_variable_loan_financialServiceKey = () =>
        {
            financeDataManager.WasToldTo(x => x.GetVariableLoanFinancialServiceKeyByAccount(thirdPartyInvoice.AccountKey));
        };

        It should_post_the_legal_fee_capitalised_transaction = () =>
        {
            loanTransactionManager.WasToldTo(x =>
                x.PostTransaction(financialServiceKey, Interfaces.FinanceDomain.Enum.LoanTransactionTypeEnum.NonCapitalisedLegalFeeTransaction,
                (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, Param.IsAny<DateTime>(), thirdPartyInvoice.SahlReference, metadata.UserName));
        };

        It should_raise_a_capistalised_legal_fee_transaction_posted_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param<TransactionsProcessedForThirdPartyInvoicePaymentEvent>.Matches(e =>
                    e.AccountKey == thirdPartyInvoice.AccountKey &&
                    e.FinancialServiceKey == financialServiceKey &&
                    e.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey
                ),
                command.ThirdPartyInvoiceKey,
                (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice, metadata));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };
    }
}
