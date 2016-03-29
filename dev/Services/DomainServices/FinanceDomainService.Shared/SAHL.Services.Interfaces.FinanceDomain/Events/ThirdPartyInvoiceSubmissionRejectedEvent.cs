using SAHL.Core.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceSubmissionRejectedEvent : Event
    {
        public int AccountNumber { get; protected set; }

        public AttorneyInvoiceDocumentModel InvoiceDocument { get; protected set; }

        public IEnumerable<string> RejectionReasons { get; protected set; }

        public ThirdPartyInvoiceSubmissionRejectedEvent(DateTime date, int accountNumber, AttorneyInvoiceDocumentModel invoiceDocument, IEnumerable<string> rejectionReasons)
            : base(date)
        {
            this.AccountNumber = accountNumber;
            this.InvoiceDocument = invoiceDocument;
            this.RejectionReasons = rejectionReasons;
        }
    }
}