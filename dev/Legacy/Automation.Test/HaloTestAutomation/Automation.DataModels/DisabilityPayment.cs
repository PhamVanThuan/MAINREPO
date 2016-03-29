using System;

namespace Automation.DataModels
{
    public sealed class DisabilityPaymentSchedule : Record, IDataModel
    {
        public int DisabilityPaymentKey { get; set; }

        public int DisabilityClaimKey { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public int DisabilityPaymentStatusKey { get; set; }
    }
}