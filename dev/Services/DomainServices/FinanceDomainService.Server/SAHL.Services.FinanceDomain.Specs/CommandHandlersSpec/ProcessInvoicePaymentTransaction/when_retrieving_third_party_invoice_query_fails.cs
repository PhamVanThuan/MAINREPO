using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
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
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ProcessInvoicePaymentTransaction
{
    public class when_retrieving_third_party_invoice_query_fails : WithFakes
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
        private static IThirdPartyInvoiceDataManager dataManager;

        private Establish context = () =>
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

             var getThirdPartyInvoiceMessages = SystemMessageCollection.Empty();
             serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param.IsAny<GetThirdPartyInvoiceQuery>())).Return<GetThirdPartyInvoiceQuery>(y =>
             {
                 y.Result = new ServiceQueryResult<GetThirdPartyInvoiceQueryResult>(new List<GetThirdPartyInvoiceQueryResult> { });
                 getThirdPartyInvoiceMessages.AddMessage(new SystemMessage("An internal error occured", SystemMessageSeverityEnum.Error));
                 return getThirdPartyInvoiceMessages;
             });
         };

        private Because of = () =>
         {
             messages = handler.HandleCommand(command, metadata);
         };

        private It should_get_the_third_party_invoice_using_the_third_party_invoice_key = () =>
         {
             serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetThirdPartyInvoiceQuery>.Matches(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey)));
         };

        private It should_return_error_messages = () =>
         {
             messages.HasErrors.ShouldBeTrue();
             messages.AllMessages.First().Message.ShouldEqual("An internal error occured");
         };

        private It should_get_the_accounts_variable_loan_financialServiceKey = () =>
         {
             financeDataManager.WasNotToldTo(x => x.GetVariableLoanFinancialServiceKeyByAccount(Param.IsAny<int>()));
         };

        private It should_not_post_the_transaction = () =>
         {
             loanTransactionManager.WasNotToldTo(x =>
                 x.PostTransaction(Param.IsAny<int>(), Param.IsAny<LoanTransactionTypeEnum>(), Param.IsAny<decimal>(), Param.IsAny<DateTime>(), Param.IsAny<string>(),
                 Param.IsAny<string>()));
         };

        private It should_not_raise_an_event = () =>
         {
             eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                 Param.IsAny<TransactionsProcessedForThirdPartyInvoicePaymentEvent>(),
                 Param.IsAny<int>(),
                 Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
         };
    }
}