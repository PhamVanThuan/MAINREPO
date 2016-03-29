using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class CompensateLodgeDisabilityClaimEvent : Event
    {
        public CompensateLodgeDisabilityClaimEvent(DateTime date, int disabilityClaimKey)
            : base(date)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }

        public int DisabilityClaimKey { get; protected set; }
    }
}