using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class AddThirdPartyInvoiceLineItemsCommandHandler : IServiceCommandHandler<AddThirdPartyInvoiceLineItemsCommand>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public AddThirdPartyInvoiceLineItemsCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            //TODO: rule to ensure that an invoice against a closed account can only have non-capitalised line items
        }

        public ISystemMessageCollection HandleCommand(AddThirdPartyInvoiceLineItemsCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            foreach (var invoiceLineItem in command.NewInvoiceLineItems)
            {
                thirdPartyInvoiceDataManager.AddInvoiceLineItem(invoiceLineItem);
            }
            return messages;
        }
    }
}