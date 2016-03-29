using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceRejectedPostApprovalEvent : ThirdPartyInvoiceRejectedEvent
    {
        public ThirdPartyInvoiceRejectedPostApprovalEvent(DateTime date, int accountNumber, string attorneyEmailAddress, string invoiceNumber, string rejectedBy, string rejectionComments
            , string sahlReferenceNumber, int thirdPartyInvoiceKey, string emailSubject, Guid? thirdPartyId)
            : base(date, accountNumber, attorneyEmailAddress, invoiceNumber, rejectedBy, rejectionComments
            , sahlReferenceNumber, thirdPartyInvoiceKey, emailSubject, thirdPartyId)
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
        
    }
}

