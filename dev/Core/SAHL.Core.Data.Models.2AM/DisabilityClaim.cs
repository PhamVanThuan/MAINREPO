using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisabilityClaimDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DisabilityClaimDataModel(int accountKey, int legalEntityKey, DateTime dateClaimReceived, DateTime? lastDateWorked, DateTime? dateOfDiagnosis, string claimantOccupation, int? disabilityTypeKey, string otherDisabilityComments, DateTime? expectedReturnToWorkDate, int disabilityClaimStatusKey, DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate)
        {
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
		[JsonConstructor]
        public DisabilityClaimDataModel(int disabilityClaimKey, int accountKey, int legalEntityKey, DateTime dateClaimReceived, DateTime? lastDateWorked, DateTime? dateOfDiagnosis, string claimantOccupation, int? disabilityTypeKey, string otherDisabilityComments, DateTime? expectedReturnToWorkDate, int disabilityClaimStatusKey, DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate)
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

        public int DisabilityClaimKey { get; set; }

        public int AccountKey { get; set; }

        public int LegalEntityKey { get; set; }

        public DateTime DateClaimReceived { get; set; }

        public DateTime? LastDateWorked { get; set; }

        public DateTime? DateOfDiagnosis { get; set; }

        public string ClaimantOccupation { get; set; }

        public int? DisabilityTypeKey { get; set; }

        public string OtherDisabilityComments { get; set; }

        public DateTime? ExpectedReturnToWorkDate { get; set; }

        public int DisabilityClaimStatusKey { get; set; }

        public DateTime? PaymentStartDate { get; set; }

        public int? NumberOfInstalmentsAuthorised { get; set; }

        public DateTime? PaymentEndDate { get; set; }

        public void SetKey(int key)
        {
            this.DisabilityClaimKey =  key;
        }
    }
}