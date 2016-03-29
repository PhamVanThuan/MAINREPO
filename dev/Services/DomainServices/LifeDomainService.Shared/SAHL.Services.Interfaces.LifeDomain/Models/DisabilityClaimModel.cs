using SAHL.Core.Validation;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Models
{
    public class DisabilityClaimModel : ValidatableModel
    {
        public DisabilityClaimModel(int disabilityClaimKey, int accountKey, int legalEntityKey, DateTime dateClaimReceived, DateTime? lastDateWorked,
            DateTime? dateOfDiagnosis, string claimantOccupation, int? disabilityTypeKey, string otherDisabilityComments, DateTime? expectedReturnToWorkDate,
            int disabilityClaimStatusKey, DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.AccountKey = accountKey;
            this.LegalEntityKey = legalEntityKey;
            this.DateClaimReceived = dateClaimReceived;
            this.LastDateWorked = lastDateWorked;
            this.DateOfDiagnosis = dateOfDiagnosis;
            this.ClaimantOccupation = claimantOccupation;
            this.DisabilityTypeKey = disabilityTypeKey;
            this.OtherDisabilityComments = otherDisabilityComments;
            this.ExpectedReturnToWorkDate = expectedReturnToWorkDate;
            this.DisabilityClaimStatusKey = disabilityClaimStatusKey;
            this.PaymentStartDate = paymentStartDate;
            this.NumberOfInstalmentsAuthorised = numberOfInstalmentsAuthorised;
            this.PaymentEndDate = paymentEndDate;
        }

        public int DisabilityClaimKey { get; protected set; }

        public int AccountKey { get; protected set; }

        public int LegalEntityKey { get; protected set; }

        public DateTime DateClaimReceived { get; protected set; }

        public DateTime? LastDateWorked { get; protected set; }

        public DateTime? DateOfDiagnosis { get; protected set; }

        public string ClaimantOccupation { get; protected set; }

        public int? DisabilityTypeKey { get; protected set; }

        public string OtherDisabilityComments { get; set; }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }

        public int DisabilityClaimStatusKey { get; protected set; }

        public DateTime? PaymentStartDate { get; protected set; }

        public int? NumberOfInstalmentsAuthorised { get; protected set; }

        public DateTime? PaymentEndDate { get; protected set; }
    }
}