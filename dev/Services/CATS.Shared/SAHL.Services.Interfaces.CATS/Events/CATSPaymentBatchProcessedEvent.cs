using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.CATS.Events
{
    public class CATSPaymentBatchProcessedEvent : Event
    {
        public int BatchReferenceNumber { get; protected set; }

        public CATSPaymentBatchProcessedEvent(DateTime date, int batchReferenceNumber)
            : base(date)
        {
            BatchReferenceNumber = batchReferenceNumber;
        }
    }
}
