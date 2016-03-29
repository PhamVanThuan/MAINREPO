using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimRepudiatedEvent : Event
    {
        public DisabilityClaimRepudiatedEvent(DateTime date, int disabilityClaimKey, IEnumerable<int> reasonKeys)
            : base(date)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.ReasonKeys = reasonKeys;
        }

        public int DisabilityClaimKey { get; protected set; }

        public IEnumerable<int> ReasonKeys { get; protected set; }
    }
}