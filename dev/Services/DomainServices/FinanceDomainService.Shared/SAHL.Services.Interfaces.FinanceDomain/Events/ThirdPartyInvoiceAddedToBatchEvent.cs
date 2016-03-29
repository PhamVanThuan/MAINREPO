using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceAddedToBatchEvent : Event
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public int CATSPaymentBatchKey { get; protected set; }
        public decimal PaidInvoiceAmount { get; protected set; }

        public ThirdPartyInvoiceAddedToBatchEvent(DateTime date, int catsPaymentBatchKey, int thirdPartyInvoiceKey, decimal paidInvoiceAmount)
            : base(date)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            PaidInvoiceAmount = paidInvoiceAmount;
            CATSPaymentBatchKey = catsPaymentBatchKey;
        }
    }
}
