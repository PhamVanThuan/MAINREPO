using System;
namespace Automation.DataModels
{
    public sealed class DisabilityClaim : Record, IDataModel
    {
        public int LoanAccountKey { get; set; }
        public int DisabilityClaimKey { get; set; }
        public int AccountKey { get; set; }
        public int LegalEntityKey { get; set; }
        public DateTime DateClaimReceived{ get; set; }
        public DateTime? LastDateWorked { get; set; }
        public DateTime? DateOfDiagnosis { get; set; }
        public string ClaimantOccupation { get; set; }
        public int? DisabilityTypeKey { get; set; }
        public string OtherDisabilityComments { get; set; }
        public DateTime? ExpectedReturnToWorkDate { get; set; }
        public int? DisabilityClaimStatusKey { get; set; }
        public int? NumberOfInstalmentsAuthorised { get; set; }
        public DateTime? PaymentEndDate { get; set; }
    }
}