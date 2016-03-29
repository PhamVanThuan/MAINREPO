using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class AmendThirdPartyInvoiceLineItemsCommandHandler : IServiceCommandHandler<AmendThirdPartyInvoiceLineItemsCommand>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public AmendThirdPartyInvoiceLineItemsCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            //TODO: rule to ensure that an invoice against a closed account can only have non-capitalised line items
        }

        public ISystemMessageCollection HandleCommand(AmendThirdPartyInvoiceLineItemsCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            foreach (var lineItem in command.lineItemsToUpdate)
            {
                thirdPartyInvoiceDataManager.AmendInvoiceLineItem(lineItem);
            }
            return messages;
        }
    }
}