using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimLodgedEvent : Event
    {
        public DisabilityClaimLodgedEvent(DateTime date, int lifeAccountKey, int claimantLegalEntityKey)
            : base(date)
        {
            this.LifeAccountKey = lifeAccountKey;
            this.ClaimantLegalEntityKey = claimantLegalEntityKey;
        }

        public int LifeAccountKey { get; protected set; }

        public int ClaimantLegalEntityKey { get; protected set; }
    }
}