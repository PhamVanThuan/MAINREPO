using SAHL.Core.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceSubmissionAcceptedEvent : Event
    {
        public string SAHLReference { get; protected set; }

        public int AccountNumber { get; protected set; }

        public AttorneyInvoiceDocumentModel InvoiceDocument { get; protected set; }

        public ThirdPartyInvoiceSubmissionAcceptedEvent(DateTime date, string sahlReference, int accountNumber, AttorneyInvoiceDocumentModel invoiceDocument)
            : base(date)
        {
            this.AccountNumber = accountNumber;
            this.InvoiceDocument = invoiceDocument;
            this.SAHLReference = sahlReference;
        }
    }
}