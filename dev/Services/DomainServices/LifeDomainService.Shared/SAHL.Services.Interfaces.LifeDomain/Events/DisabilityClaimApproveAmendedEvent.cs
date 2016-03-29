using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimApproveAmendedEvent : Event
    {
        public DisabilityClaimApproveAmendedEvent(DateTime date, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation, DateTime? expectedReturnToWorkDate)
            : base(date)
        {
            this.DisabilityTypeKey = disabilityTypeKey;
            this.OtherDisabilityComments = otherDisabilityComments;
            this.ClaimantOccupation = claimantOccupation;
            this.ExpectedReturnToWorkDate = expectedReturnToWorkDate;
        }

        public int DisabilityTypeKey { get; protected set; }

        public string OtherDisabilityComments { get; protected set; }

        public string ClaimantOccupation { get; protected set; }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }
    }
}