using ObjectMaps;
using ObjectMaps.Pages;
namespace BuildingBlocks.Presenters.DisabilityClaim
{
    public class RepudiateDisabilityClaim : RepudiateDisabilityClaimControls
    {
        public void SelectRepudiationReason(string reason)
        {
            base.AvailableReasons.Select(reason);
            base.AddReason.Click();
        }

        public void SubmitDisabilityClaimRepudiation()
        {
            CommonIEDialogHandler.SelectOK(base.Submit);
            base.Document.DomContainer.WaitForComplete();
        }

        public void CancelDisabilityClaimBeforeRepudiation()
        {
            base.Cancel.Click();
        }

        public string SubmitDisabilityClaimRepudiationWithoutSelectingAReason()
        {
            var message = CommonIEDialogHandler.SelectOKInAlertDialogAndReturnMessage(base.Submit);
            base.Document.DomContainer.WaitForComplete();
            return message;
        }
    }
}