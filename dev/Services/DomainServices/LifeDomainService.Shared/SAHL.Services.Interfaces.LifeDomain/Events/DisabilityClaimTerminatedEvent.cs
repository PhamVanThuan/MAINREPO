using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimTerminatedEvent : Event
    {
        public DisabilityClaimTerminatedEvent(DateTime date, int disabilityClaimKey, int reasonKey)
            : base(date)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.ReasonKey = reasonKey;
        }

        public int DisabilityClaimKey { get; protected set; }

        public int ReasonKey { get; protected set; }
    }
}