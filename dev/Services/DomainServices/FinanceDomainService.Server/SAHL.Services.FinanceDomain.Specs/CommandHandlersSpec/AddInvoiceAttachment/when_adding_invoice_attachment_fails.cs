using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AddInvoiceAttachment
{
    public class when_adding_invoice_attachment_fails : WithFakes
    {
        private static AddInvoiceAttachmentCommand command;
        private static AddInvoiceAttachmentCommandHandler handler;
        private static ISystemMessageCollection messages;
        private static ISystemMessageCollection storeManagerMessages;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static string loanNumber, mailFrom, mailSubject;
        private static int thirdPartyInvoiceKey;
        private static DateTime dateReceived;
        private static InvoiceAttachment attachment;
        private static string fileName, fileExtension;
        private static Interfaces.FinanceDomain.Model.AttorneyInvoiceDocumentModel invoiceDocument;
        private static IDocumentManagerServiceClient documentManager;

        private Establish context = () =>
        {
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            documentManager = An<IDocumentManagerServiceClient>();
            messages = SystemMessageCollection.Empty();
            storeManagerMessages = SystemMessageCollection.Empty();

            storeManagerMessages.AddMessage(new SystemMessage("Failed: could not store invoice attachment", SystemMessageSeverityEnum.Error));

            loanNumber = "A12345";
            thirdPartyInvoiceKey = 211;
            mailFrom = "lawyer@attorneys.co.za";
            mailSubject = "342 My invoice";
            fileName = "My Invoice Document.pdf";
            fileExtension = "pdf";
            dateReceived = DateTime.Now.AddDays(-2);
            attachment = new InvoiceAttachment(fileName, fileExtension, "Something here");

            invoiceDocument = new Interfaces.FinanceDomain.Model.AttorneyInvoiceDocumentModel(loanNumber, dateReceived,
                     DateTime.Now, mailFrom, mailSubject, fileName, fileExtension, "", attachment.FileContents);

            command = new AddInvoiceAttachmentCommand(invoiceDocument, thirdPartyInvoiceKey);
            handler = new AddInvoiceAttachmentCommandHandler(documentManager);
            documentManager.WhenToldTo(x => x.PerformCommand(Param.IsAny<StoreAttorneyInvoiceCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(storeManagerMessages);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_perform_command_to_store_invoice_documents = () =>
        {
            documentManager.WasToldTo(x => x.PerformCommand(Param.IsAny<StoreAttorneyInvoiceCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_return_messages = () =>
        {
            messages.AllMessages.Any().ShouldBeTrue();
        };
    }
}