using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceRemovedFromBatchEvent : Event
    {
        public int CatsPaymentBatchKey { get; protected set; }
        public int ThirdPartyInvoiceKey { get; protected set; }
        public decimal InvoiceAmount { get; protected set; }

        public ThirdPartyInvoiceRemovedFromBatchEvent(DateTime date, int catsPaymentBatchKey, int thirdPartyInvoiceKey, decimal invoiceAmount) 
            : base(date)
        {
            CatsPaymentBatchKey = catsPaymentBatchKey;
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            InvoiceAmount = invoiceAmount;
        }
    }
}
