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
    public class EscalateThirdPartyInvoiceForApprovalCommandHandler : IDomainServiceCommandHandler<EscalateThirdPartyInvoiceForApprovalCommand, ThirdPartyInvoiceEscalatedForApprovalEvent>
    {
        private IThirdPartyInvoiceDataManager dataManager;
        private IEventRaiser eventRaiser;

        public EscalateThirdPartyInvoiceForApprovalCommandHandler(IThirdPartyInvoiceDataManager dataManager, IEventRaiser eventRaiser)
        {
            this.dataManager = dataManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(EscalateThirdPartyInvoiceForApprovalCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.dataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.AwaitingApproval);
            if (!messages.HasErrors)
            {
                var @event = new ThirdPartyInvoiceEscalatedForApprovalEvent(DateTime.Now, command.ThirdPartyInvoiceKey, metadata.UserOrganisationStructureKey.GetValueOrDefault(),
                    command.UOSKeyForEscalatedUser);
                eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }
            return messages;
        }
    }
}