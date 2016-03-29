using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class RejectThirdPartyInvoiceCommandHandler : IDomainServiceCommandHandler<RejectThirdPartyInvoiceCommand, ThirdPartyInvoiceRejectedEvent>
    {
        private IEventRaiser eventRaiser;
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public RejectThirdPartyInvoiceCommandHandler(IEventRaiser eventRaiser, IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.eventRaiser = eventRaiser;
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public ISystemMessageCollection HandleCommand(RejectThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            var rejectedBy = metadata.UserName;

            var emailSubject = this.thirdPartyInvoiceDataManager.GetThirdPartyInvoiceEmailSubject(command.ThirdPartyInvoiceKey);
            var model = this.thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(command.ThirdPartyInvoiceKey);
            var hasBeenApproved = thirdPartyInvoiceDataManager.HasThirdPartyInvoiceBeenApproved(command.ThirdPartyInvoiceKey);
            this.thirdPartyInvoiceDataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Rejected);
            if (!messages.HasErrors)
            {
                ThirdPartyInvoiceRejectedEvent @event;
                if (hasBeenApproved)
                {
                    @event = new ThirdPartyInvoiceRejectedPostApprovalEvent(DateTime.Now, model.AccountKey, model.ReceivedFromEmailAddress, model.InvoiceNumber,
                    rejectedBy, command.RejectionComments, model.SahlReference, command.ThirdPartyInvoiceKey, emailSubject, model.ThirdPartyId);
                }
                else
                {
                    @event = new ThirdPartyInvoiceRejectedPreApprovalEvent(DateTime.Now, model.AccountKey, model.ReceivedFromEmailAddress, model.InvoiceNumber,
                    rejectedBy, command.RejectionComments, model.SahlReference, command.ThirdPartyInvoiceKey, emailSubject, model.ThirdPartyId);
                }
                
                this.eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }

            return messages;
        }
    }
}