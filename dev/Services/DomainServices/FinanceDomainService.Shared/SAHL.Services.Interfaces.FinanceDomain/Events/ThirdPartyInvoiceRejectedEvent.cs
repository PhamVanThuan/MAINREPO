using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceRejectedEvent: Event
    {
        public ThirdPartyInvoiceRejectedEvent(DateTime date, int accountNumber, string attorneyEmailAddress, string invoiceNumber, string rejectedBy, string rejectionComments
            , string sahlReferenceNumber, int thirdPartyInvoiceKey, string emailSubject, Guid? thirdPartyId)
            : base(date)
        {
            this.AccountNumber = accountNumber;
            this.AttorneyEmailAddress = attorneyEmailAddress;
            this.InvoiceNumber = invoiceNumber;
            this.RejectedBy = rejectedBy;
            this.RejectionComments = rejectionComments;
            this.SAHLReferenceNumber = sahlReferenceNumber;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.EmailSubject = emailSubject;
            this.ThirdPartyId = thirdPartyId;
        }

        public int AccountNumber { get; protected set; }

        public string AttorneyEmailAddress { get; protected set; }

        public string InvoiceNumber { get; protected set; }

        public string RejectedBy { get; protected set; }

        public string RejectionComments { get; protected set; }

        public string SAHLReferenceNumber { get; protected set; }

        public int ThirdPartyInvoiceKey { get; protected set; }

        public string EmailSubject { get; set; }

        public Guid? ThirdPartyId { get; set; }
    }
}
