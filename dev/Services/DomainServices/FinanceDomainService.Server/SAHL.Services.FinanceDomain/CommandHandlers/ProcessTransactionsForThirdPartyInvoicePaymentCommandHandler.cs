using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class ProcessTransactionsForThirdPartyInvoicePaymentCommandHandler : IDomainServiceCommandHandler<ProcessTransactionsForThirdPartyInvoicePaymentCommand, TransactionsProcessedForThirdPartyInvoicePaymentEvent>
    {
        private ILoanTransactionManager loanTransactionManager;
        private IFinanceDataManager financeDataManager;
        private IEventRaiser eventRaiser;
        private IServiceQueryRouter serviceQueryRouter;
        private IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private IThirdPartyInvoiceDataManager dataManager;

        public ProcessTransactionsForThirdPartyInvoicePaymentCommandHandler(ILoanTransactionManager loanTransactionManager, IFinanceDataManager financeDataManager,
            IEventRaiser eventRaiser, IServiceQueryRouter serviceQueryRouter, IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager, IThirdPartyInvoiceDataManager dataManager)
        {
            this.dataManager = dataManager;
            this.loanTransactionManager = loanTransactionManager;
            this.financeDataManager = financeDataManager;
            this.eventRaiser = eventRaiser;
            this.serviceQueryRouter = serviceQueryRouter;
            this.domainRuleManager = domainRuleManager;
            domainRuleManager.RegisterPartialRule<IThirdPartyInvoiceRuleModel>(new InvoicePaymentShouldBeBeingProcessedRule(dataManager));
        }

        public ISystemMessageCollection HandleCommand(ProcessTransactionsForThirdPartyInvoicePaymentCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command);
            if (messages.HasErrors)
            {
                return messages;
            }

            var getThirdPartyInvoiceQuery = new GetThirdPartyInvoiceQuery(command.ThirdPartyInvoiceKey);
            messages.Aggregate(serviceQueryRouter.HandleQuery(getThirdPartyInvoiceQuery));
            if (messages.HasErrors)
            {
                return messages;
            }

            var thirdPartyInvoice = getThirdPartyInvoiceQuery.Result.Results.First();
            var transactionType = thirdPartyInvoice.CapitaliseInvoice.GetValueOrDefault() ? LoanTransactionTypeEnum.CapitalisedLegalFeeTransaction
                : LoanTransactionTypeEnum.NonCapitalisedLegalFeeTransaction;

            var financialServiceKey = financeDataManager.GetVariableLoanFinancialServiceKeyByAccount(thirdPartyInvoice.AccountKey);
            if (financialServiceKey == null)
            {
                messages.AddMessage(
                    new SystemMessage(string.Format("No 'variable loan' financial service record could be found for the account {0}", thirdPartyInvoice.AccountKey),
                    SystemMessageSeverityEnum.Error));
                return messages;
            }

            messages.Aggregate(
                loanTransactionManager.PostTransaction((int)financialServiceKey, transactionType, (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, DateTime.Now,
                    thirdPartyInvoice.SahlReference, metadata.UserName)
            );

            if (!messages.HasErrors)
            {
                var @event = new TransactionsProcessedForThirdPartyInvoicePaymentEvent((int)financialServiceKey, thirdPartyInvoice.AccountKey, command.ThirdPartyInvoiceKey,
                (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, DateTime.Now, thirdPartyInvoice.SahlReference, metadata.UserName);
                eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }

            return messages;
        }
    }
}