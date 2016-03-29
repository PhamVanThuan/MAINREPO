using ObjectMaps.Pages;
namespace BuildingBlocks.Presenters.DisabilityClaim
{
    public class ApproveDisabilityClaim : ApproveDisabilityClaimControls
    {
        public void AddNumberOfInstalments(int numberOfInstalmentsAuthorised)
        {
            base.NumberOfInstalmentsAuthorised.Value = numberOfInstalmentsAuthorised.ToString();
            base.NumberOfInstalmentsAuthorised.KeyUp();
        }

        public void SubmitDisabilityClaim()
        {
            base.Submit.Click();
        }

        public void CancelDisabilityClaimBeforeSubmission()
        {
            base.Cancel.Click();
        }
    }
}