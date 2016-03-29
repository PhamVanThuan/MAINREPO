using SAHL.Core.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimCapturedEvent : Event
    {
        public DisabilityClaimCapturedEvent(DateTime date, DateTime dateOfDiagnosis, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation,
            DateTime lastDateWorked, DateTime? expectedReturnToWorkDate)
            : base(date)
        {
            this.DateOfDiagnosis = dateOfDiagnosis;
            this.DisabilityTypeKey = disabilityTypeKey;
            this.OtherDisabilityComments = otherDisabilityComments;
            this.ClaimantOccupation = claimantOccupation;
            this.LastDateWorked = lastDateWorked;
            this.ExpectedReturnToWorkDate = expectedReturnToWorkDate;
        }

        public DateTime DateOfDiagnosis { get; protected set; }

        public int DisabilityTypeKey { get; protected set; }

        public string OtherDisabilityComments { get; protected set; }

        public string ClaimantOccupation { get; protected set; }

        public DateTime LastDateWorked { get; protected set; }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }
    }
}