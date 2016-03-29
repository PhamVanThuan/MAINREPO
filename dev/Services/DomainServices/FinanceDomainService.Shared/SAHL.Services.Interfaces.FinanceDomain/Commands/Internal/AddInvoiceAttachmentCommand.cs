using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class AddInvoiceAttachmentCommand : ServiceCommand, IFinanceDomainInternalCommand, IRequiresThirdPartyInvoice
    {
        [Required]
        public AttorneyInvoiceDocumentModel InvoiceDocument { get; protected set; }

        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public AddInvoiceAttachmentCommand(AttorneyInvoiceDocumentModel invoiceDocument, int thirdPartyInvoiceKey)
        {
            this.InvoiceDocument = invoiceDocument;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}