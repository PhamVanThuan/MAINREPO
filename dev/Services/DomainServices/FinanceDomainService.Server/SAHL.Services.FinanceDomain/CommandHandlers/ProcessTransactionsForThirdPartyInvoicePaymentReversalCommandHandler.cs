using System;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler :
        IDomainServiceCommandHandler<ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand, TransactionProcessedForThirdPartyInvoicePaymentReversalEvent>
    {
        private ILoanTransactionManager loanTransactionManager;
        private IFinanceDataManager financeDataManager;
        private IEventRaiser eventRaiser;
        private IServiceQueryRouter serviceQueryRouter;

        public ProcessTransactionsForThirdPartyInvoicePaymentReversalCommandHandler(ILoanTransactionManager loanTransactionManager, IFinanceDataManager financeDataManager,
            IEventRaiser eventRaiser, IServiceQueryRouter serviceQueryRouter)
        {
            this.loanTransactionManager = loanTransactionManager;
            this.financeDataManager = financeDataManager;
            this.eventRaiser = eventRaiser;
            this.serviceQueryRouter = serviceQueryRouter;
        }

        public ISystemMessageCollection HandleCommand(ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var getThirdPartyInvoiceQuery = new GetThirdPartyInvoiceQuery(command.ThirdPartyInvoiceKey);
            messages.Aggregate(serviceQueryRouter.HandleQuery(getThirdPartyInvoiceQuery));
            if (!messages.HasErrors)
            {
                var thirdPartyInvoice = getThirdPartyInvoiceQuery.Result.Results.First();

                var financialTransactionKey = financeDataManager.GetFinancialTransactionKeyByReference(thirdPartyInvoice.SahlReference);
                if (financialTransactionKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to retrieve the financial transaction key", SystemMessageSeverityEnum.Warning));
                    return messages;
                }

                messages.Aggregate(loanTransactionManager.PostReversalTransaction(financialTransactionKey, metadata.UserName));
                if (!messages.HasErrors)
                {
                    var @event = new TransactionProcessedForThirdPartyInvoicePaymentReversalEvent(financialTransactionKey, thirdPartyInvoice.AccountKey, thirdPartyInvoice.ThirdPartyInvoiceKey,
                        (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, DateTime.Now, thirdPartyInvoice.SahlReference, metadata.UserName
                        );
                    eventRaiser.RaiseEvent(DateTime.Now, @event, thirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
                }
            }

            return messages;
        }
    }
}