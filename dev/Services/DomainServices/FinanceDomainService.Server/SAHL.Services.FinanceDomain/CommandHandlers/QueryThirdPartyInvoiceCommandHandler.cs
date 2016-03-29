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
    public class QueryThirdPartyInvoiceCommandHandler : IDomainServiceCommandHandler<QueryThirdPartyInvoiceCommand, ThirdPartyInvoiceQueriedEvent>
    {
        private IThirdPartyInvoiceDataManager dataManager;
        private IEventRaiser eventRaiser;

        public QueryThirdPartyInvoiceCommandHandler(IThirdPartyInvoiceDataManager dataManager, IEventRaiser eventRaiser)
        {
            this.dataManager = dataManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(QueryThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            bool queryPostApproval = false;
            queryPostApproval = dataManager.HasThirdPartyInvoiceBeenApproved(command.ThirdPartyInvoiceKey);
            this.dataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Captured);
            if (!messages.HasErrors)
            {
                if (queryPostApproval)
                {
                    eventRaiser.RaiseEvent(DateTime.Now, new ThirdPartyInvoiceQueriedPostApprovalEvent(DateTime.Now, command.ThirdPartyInvoiceKey, metadata.UserName, command.QueryComments)
                        , command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
                }
                else
                {
                    eventRaiser.RaiseEvent(DateTime.Now, new ThirdPartyInvoiceQueriedPreApprovalEvent(DateTime.Now, command.ThirdPartyInvoiceKey, metadata.UserName, command.QueryComments)
                        , command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
                }
            }
            return messages;
        }
    }
}