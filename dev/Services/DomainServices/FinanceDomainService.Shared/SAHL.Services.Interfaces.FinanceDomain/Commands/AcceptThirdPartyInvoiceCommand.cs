using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class AcceptThirdPartyInvoiceCommand : ServiceCommand, IFinanceDomainCommand, IAccountRuleModel
    {
        [Required]
        public int AccountNumber { get; protected set; }

        [Required]
        public AttorneyInvoiceDocumentModel InvoiceDocument { get; protected set; }

        [Required]
        public int ThirdPartyTypeKey { get; protected set; }

        public AcceptThirdPartyInvoiceCommand(int accountNumber, AttorneyInvoiceDocumentModel invoiceDocument, int thirdPartyTypeKey)
        {
            this.AccountNumber = accountNumber;
            this.InvoiceDocument = invoiceDocument;
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
        }
    }
}