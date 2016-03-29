using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.CATS.Events
{
    public class SummarisedPaymentsToRecipientEvent : Event
    {
        public IEnumerable<CATSPaymentBatchItemDataModel> InvoicePayments { get; protected set; }

        public string EmailAddress { get; protected set; }

        public SummarisedPaymentsToRecipientEvent(DateTime date, string emailAddress, IEnumerable<CATSPaymentBatchItemDataModel> invoicePayments)
            : base(date)
        {
            EmailAddress = emailAddress;
            InvoicePayments = invoicePayments;
        }
    }
}
