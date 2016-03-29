using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimApprovedEvent : Event
    {
        public DisabilityClaimApprovedEvent(DateTime date, DateTime paymentStartDate, int numberOfInstalmentsAuthorised, DateTime paymentEndDate)
            : base(date)
        {
            this.PaymentStartDate = paymentStartDate;
            this.NumberOfInstalmentsAuthorised = numberOfInstalmentsAuthorised;
            this.PaymentEndDate = paymentEndDate;
        }

        public DateTime PaymentStartDate { get; protected set; }

        public int NumberOfInstalmentsAuthorised { get; protected set; }

        public DateTime PaymentEndDate { get; protected set; }
    }
}