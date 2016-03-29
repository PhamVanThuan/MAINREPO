using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ProcessInvoicePaymentTransactionReversal
{
    public class when_retrieving_invoice_fails : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static IFinanceDataManager financeDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler handler;
        private static ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand command;

        Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            loanTransactionManager = An<ILoanTransactionManager>();
            financeDataManager = An<IFinanceDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            metadata = An<IServiceRequestMetadata>();

            handler = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler(loanTransactionManager, financeDataManager, eventRaiser, serviceQueryRouter);
            command = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(1408);

            var thirdPartyInvoiceList = new List<GetThirdPartyInvoiceQueryResult> { };
            var results = new ServiceQueryResult<GetThirdPartyInvoiceQueryResult>(thirdPartyInvoiceList);
            var errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Failed to retrieve third party invoice", SystemMessageSeverityEnum.Error));
            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param.IsAny<GetThirdPartyInvoiceQuery>())).Return<GetThirdPartyInvoiceQuery>(y =>
            {
                y.Result = results;
                return errorMessages;
            });

        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        It should_get_the_third_party_invoice_using_the_third_party_key = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetThirdPartyInvoiceQuery>.Matches(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey)));
        };

        It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

    }
}
