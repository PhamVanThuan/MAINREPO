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
    public class MarkThirdPartyInvoiceAsPaidCommandHandler : IDomainServiceCommandHandler<MarkThirdPartyInvoiceAsPaidCommand, ThirdPartyInvoiceMarkedAsPaidEvent>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private IEventRaiser eventRaiser;

        public MarkThirdPartyInvoiceAsPaidCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager, IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager,
            IEventRaiser eventRaiser)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.domainRuleManager = domainRuleManager;
            this.eventRaiser = eventRaiser;
            domainRuleManager.RegisterPartialRule<IThirdPartyInvoiceRuleModel>(new InvoicePaymentShouldBeBeingProcessedRule(thirdPartyInvoiceDataManager));
        }

        public ISystemMessageCollection HandleCommand(MarkThirdPartyInvoiceAsPaidCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command);
            if (!messages.HasErrors)
            {
                thirdPartyInvoiceDataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Paid);
                var @event = new ThirdPartyInvoiceMarkedAsPaidEvent(DateTime.Now, command.ThirdPartyInvoiceKey);
                eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }

            return messages;
        }
    }
}