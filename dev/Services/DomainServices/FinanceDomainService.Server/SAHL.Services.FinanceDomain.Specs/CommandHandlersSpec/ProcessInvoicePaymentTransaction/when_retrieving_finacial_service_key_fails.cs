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
    public class when_retrieving_finacial_service_key_fails : WithFakes
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

            thirdPartyInvoice = new GetThirdPartyInvoiceQueryResult
            {
                InvoiceDate = DateTime.Now,
                AccountKey = 100008,
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

            financeDataManager.WhenToldTo(x => x.GetVariableLoanFinancialServiceKeyByAccount(Param.IsAny<int>())).Return((int?)null);
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

        It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.AllMessages.First().Message.ShouldEqual(string.Format("No 'variable loan' financial service record could be found for the account {0}", thirdPartyInvoice.AccountKey));
        };

        It should_not_post_the_transaction = () =>
        {
            loanTransactionManager.WasNotToldTo(x =>
                x.PostTransaction(Param.IsAny<int>(), Param.IsAny<LoanTransactionTypeEnum>(), Param.IsAny<decimal>(), Param.IsAny<DateTime>(), Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<TransactionsProcessedForThirdPartyInvoicePaymentEvent>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
