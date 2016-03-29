using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class AddInvoiceAttachmentCommandHandler : IServiceCommandHandler<AddInvoiceAttachmentCommand>
    {
        private IDocumentManagerServiceClient documentMangerServiceClient;

        public AddInvoiceAttachmentCommandHandler(IDocumentManagerServiceClient documentMangerServiceClient)
        {
            this.documentMangerServiceClient = documentMangerServiceClient;
        }

        public ISystemMessageCollection HandleCommand(AddInvoiceAttachmentCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            ThirdPartyInvoiceDocumentModel invoiceDocument = new ThirdPartyInvoiceDocumentModel(
                command.InvoiceDocument.LoanNumber, command.InvoiceDocument.DateReceived, command.InvoiceDocument.DateProcessed,
                command.InvoiceDocument.FromEmailAddress, command.InvoiceDocument.EmailSubject, command.InvoiceDocument.InvoiceFileName,
                command.InvoiceDocument.InvoiceFileExtension, command.InvoiceDocument.Category, command.InvoiceDocument.FileContentAsBase64);

            var storeAttorneyInvoiceCommand = new StoreAttorneyInvoiceCommand(invoiceDocument, command.ThirdPartyInvoiceKey.ToString());
            messages.Aggregate(this.documentMangerServiceClient.PerformCommand(storeAttorneyInvoiceCommand, metadata));

            return messages;
        }
    }
}