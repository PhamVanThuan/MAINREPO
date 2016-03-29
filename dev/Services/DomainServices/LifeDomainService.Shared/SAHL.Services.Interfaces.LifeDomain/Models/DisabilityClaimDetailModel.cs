using SAHL.Core.Validation;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Models
{
    public class DisabilityClaimDetailModel : ValidatableModel
    {
        public DisabilityClaimDetailModel(int disabilityClaimKey, int lifeAccountKey, int mortgageLoanAccountKey, int legalEntityKey, string claimantLegalEntityDisplayName,
            DateTime dateClaimReceived, DateTime? lastDateWorked, DateTime? dateOfDiagnosis, string claimantOccupation, int? disabilityTypeKey, string disabilityType,
            string otherDisabilityComments, DateTime? expectedReturnToWorkDate, int disabilityClaimStatusKey, string disabilityClaimStatus,
            DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.LifeAccountKey = lifeAccountKey;
            this.MortgageLoanAccountKey = mortgageLoanAccountKey;
            this.LegalEntityKey = legalEntityKey;
            this.ClaimantLegalEntityDisplayName = claimantLegalEntityDisplayName;
            this.DateClaimReceived = dateClaimReceived;
            this.LastDateWorked = lastDateWorked;
            this.DateOfDiagnosis = dateOfDiagnosis;
            this.ClaimantOccupation = claimantOccupation;
            this.DisabilityTypeKey = disabilityTypeKey;
            this.DisabilityType = disabilityType;
            this.OtherDisabilityComments = otherDisabilityComments;
            this.ExpectedReturnToWorkDate = expectedReturnToWorkDate;
            this.DisabilityClaimStatusKey = disabilityClaimStatusKey;
            this.DisabilityClaimStatus = disabilityClaimStatus;
            this.PaymentStartDate = paymentStartDate;
            this.NumberOfInstalmentsAuthorised = numberOfInstalmentsAuthorised;
            this.PaymentEndDate = paymentEndDate;
        }

        public int DisabilityClaimKey { get; protected set; }

        public int LifeAccountKey { get; protected set; }

        public int MortgageLoanAccountKey { get; protected set; }

        public int LegalEntityKey { get; protected set; }

        public string ClaimantLegalEntityDisplayName { get; protected set; }

        public DateTime DateClaimReceived { get; protected set; }

        public DateTime? LastDateWorked { get; protected set; }

        public DateTime? DateOfDiagnosis { get; protected set; }

        public string ClaimantOccupation { get; protected set; }

        public int? DisabilityTypeKey { get; protected set; }

        public string DisabilityType { get; protected set; }

        public string OtherDisabilityComments { get; set; }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }

        public int DisabilityClaimStatusKey { get; protected set; }

        public string DisabilityClaimStatus { get; protected set; }

        public DateTime? PaymentStartDate { get; protected set; }

        public int? NumberOfInstalmentsAuthorised { get; protected set; }

        public DateTime? PaymentEndDate { get; protected set; }
    }
}