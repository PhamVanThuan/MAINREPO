using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.DisabilityClaim
{
    public class CaptureDisabilityClaim : DisabilityClaimDetailsCaptureControls
    {
        public void CaptureClaimDetails(DateTime dateOfDiagnosis, string disabilityType, string otherDisabilityComments, string claimantOccupation, DateTime lastDateWorked, DateTime expectedReturnToWorkDate)
        {
            base.DateOfDiagnosis.Value = dateOfDiagnosis.ToString(Formats.DateFormat);            
            base.DisabilityType.Select(disabilityType);
            base.OtherDisabilityComments.Value = otherDisabilityComments;
            base.ClaimantOccupation.Value = claimantOccupation;
            base.LastDateWorked.Value = lastDateWorked.ToString(Formats.DateFormat);
            base.ExpectedReturnToWorkDate.Value = expectedReturnToWorkDate.ToString(Formats.DateFormat);
        }

        public void ClearAllDetails()
        {
            base.DateOfDiagnosis.Value = string.Empty;
            base.DisabilityType.SelectByValue("-select-");
            base.OtherDisabilityComments.Value = string.Empty;
            base.ClaimantOccupation.Value = string.Empty; ;
            base.LastDateWorked.Value = string.Empty;
            base.ExpectedReturnToWorkDate.Value = string.Empty;
        }
        
        public void SubmitCapturedClaimDetails()
        {
            base.Submit.Click();
        }

        public void CancelClaimBeforeSubmission ()
        {
            base.Cancel.Click();
        }
    }
}