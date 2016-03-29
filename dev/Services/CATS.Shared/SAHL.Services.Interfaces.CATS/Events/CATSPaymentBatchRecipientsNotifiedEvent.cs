using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.CATS.Events
{
    public class CATSPaymentBatchRecipientsNotifiedEvent : Event
    {
        public IEnumerable<CATSPaymentBatchItemDataModel> SuccessfulNotifications { get; protected set; }
        public IEnumerable<CATSPaymentBatchItemDataModel> UnSuccessfulNotifications { get; protected set; }

        public CATSPaymentBatchRecipientsNotifiedEvent(DateTime date, IEnumerable<CATSPaymentBatchItemDataModel> successfulNotifications
            , IEnumerable<CATSPaymentBatchItemDataModel> unSuccessfulNotifications)
            : base(date)
        {
            SuccessfulNotifications = successfulNotifications;
            UnSuccessfulNotifications = unSuccessfulNotifications;
        }
    }
}
