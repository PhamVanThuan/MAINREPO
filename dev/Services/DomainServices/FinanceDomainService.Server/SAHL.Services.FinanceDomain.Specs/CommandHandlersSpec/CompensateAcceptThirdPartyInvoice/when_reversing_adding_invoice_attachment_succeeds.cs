using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CompensateAcceptThirdPartyInvoice
{
    public class when_reversing_adding_invoice_attachment_succeeds : WithFakes
    {
        private static CompensateAddInvoiceAttachmentCommand command;
        private static CompensateAddInvoiceAttachmentCommandHandler handler;
        private static ISystemMessageCollection messages;
        private static ISystemMessageCollection managerMessages;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static IDocumentManagerServiceClient documentManager;
        private static RemoveAttorneyInvoiceCommand removeAttorneyInvoiceCommand;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            documentManager = An<IDocumentManagerServiceClient>();
            messages = SystemMessageCollection.Empty();
            managerMessages = SystemMessageCollection.Empty();
            thirdPartyInvoiceKey = 123;
            removeAttorneyInvoiceCommand = new RemoveAttorneyInvoiceCommand(thirdPartyInvoiceKey);

            documentManager.WhenToldTo(x => x.PerformCommand(Param.IsAny<RemoveAttorneyInvoiceCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(messages);
            command = new CompensateAddInvoiceAttachmentCommand(thirdPartyInvoiceKey);
            handler = new CompensateAddInvoiceAttachmentCommandHandler(documentManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_perform_command_to_remove_invoice_document_data = () =>
        {
            documentManager.WasToldTo(x => x.PerformCommand(Param.IsAny<RemoveAttorneyInvoiceCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_return_messages = () =>
        {
            messages.AllMessages.Any().ShouldBeFalse();
        };
    }
}