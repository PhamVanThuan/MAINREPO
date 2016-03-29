using System;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class ReturnThirdPartyInvoiceToPaymentQueueCommandHandler : IDomainServiceCommandHandler<ReturnThirdPartyInvoiceToPaymentQueueCommand, ThirdPartyInvoiceReturnedToPaymentQueueEvent>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private IEventRaiser eventRaiser;

        public ReturnThirdPartyInvoiceToPaymentQueueCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager, IEventRaiser eventRaiser)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(ReturnThirdPartyInvoiceToPaymentQueueCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            thirdPartyInvoiceDataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Approved);
            if (!messages.HasErrors)
            {
                var @event = new ThirdPartyInvoiceReturnedToPaymentQueueEvent(DateTime.Now, command.ThirdPartyInvoiceKey);
                eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }
            return messages;
        }
    }
}