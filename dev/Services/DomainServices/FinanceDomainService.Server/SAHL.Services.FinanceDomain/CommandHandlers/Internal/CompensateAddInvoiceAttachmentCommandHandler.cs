using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class CompensateAddInvoiceAttachmentCommandHandler : IServiceCommandHandler<CompensateAddInvoiceAttachmentCommand>
    {
        private IDocumentManagerServiceClient documentManagerServiceClient;

        public CompensateAddInvoiceAttachmentCommandHandler(IDocumentManagerServiceClient documentMangerServiceClient)
        {
            this.documentManagerServiceClient = documentMangerServiceClient;
        }

        public ISystemMessageCollection HandleCommand(CompensateAddInvoiceAttachmentCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var removeThirdPartyInvoiceCommand = new RemoveAttorneyInvoiceCommand(command.ThirdPartyInvoiceKey);
            messages.Aggregate(this.documentManagerServiceClient.PerformCommand(removeThirdPartyInvoiceCommand, new ServiceRequestMetadata()));
            return messages;
        }
    }
}