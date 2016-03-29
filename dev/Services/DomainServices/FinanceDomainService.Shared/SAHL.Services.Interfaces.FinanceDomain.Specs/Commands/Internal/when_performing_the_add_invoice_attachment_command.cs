using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Commands.Internal
{
    public class when_performing_the_add_invoice_attachment_command : WithFakes
    {
        private static AddInvoiceAttachmentCommand command;
        private static int thirdPartyInvoiceKey;
        private static AttorneyInvoiceDocumentModel invoiceDocument;

        private Establish context = () =>
        {
            thirdPartyInvoiceKey = 1;
            invoiceDocument = new AttorneyInvoiceDocumentModel("1234", DateTime.Now, DateTime.Now, "clintons@sahomeloans.com",
                "Subject", "FileName", "tiff", "category", "base64filecontent");
        };

        private Because of = () =>
        {
            command = new AddInvoiceAttachmentCommand(invoiceDocument, thirdPartyInvoiceKey);
        };

        private It should_implement_the_requires_third_party_invoice_check = () =>
        {
            command.ShouldBeAssignableTo<IRequiresThirdPartyInvoice>();
        };
    }
}