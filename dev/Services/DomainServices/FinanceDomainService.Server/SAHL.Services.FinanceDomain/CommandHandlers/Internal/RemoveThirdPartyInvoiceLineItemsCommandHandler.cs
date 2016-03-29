using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class RemoveThirdPartyInvoiceLineItemsCommandHandler : IServiceCommandHandler<RemoveThirdPartyInvoiceLineItemsCommand>
    {
        private IThirdPartyInvoiceDataManager dataManager;

        public RemoveThirdPartyInvoiceLineItemsCommandHandler(IThirdPartyInvoiceDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(RemoveThirdPartyInvoiceLineItemsCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            foreach (var lineItem in command.lineItemsToRemove)
            {
                dataManager.RemoveInvoiceLineItem(lineItem.InvoiceLineItemKey);
            }
            return messages;
        }
    }
}