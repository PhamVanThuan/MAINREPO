using System;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class ProcessThirdPartyInvoicePaymentCommandHandler : IDomainServiceCommandHandler<ProcessThirdPartyInvoicePaymentCommand, ThirdPartyInvoicePaymentProcessedEvent>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private IEventRaiser eventRaiser;

        public ProcessThirdPartyInvoicePaymentCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager, IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager,
         IEventRaiser eventRaiser)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.domainRuleManager = domainRuleManager;
            this.eventRaiser = eventRaiser;
            domainRuleManager.RegisterPartialRule<IThirdPartyInvoiceRuleModel>(new InvoiceMustBeApprovedInOrderToBeProcessedForPaymentRule(thirdPartyInvoiceDataManager));
        }

        public ISystemMessageCollection HandleCommand(ProcessThirdPartyInvoicePaymentCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command);
            if (!messages.HasErrors)
            {
                thirdPartyInvoiceDataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.ProcessingPayment);
                var @event = new ThirdPartyInvoicePaymentProcessedEvent(DateTime.Now, command.ThirdPartyInvoiceKey);
                eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }
            return messages;
        }
    }
}