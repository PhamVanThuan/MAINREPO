using ObjectMaps;
using ObjectMaps.Pages;
namespace BuildingBlocks.Presenters.DisabilityClaim
{
    public class TerminateDisabilityClaim : TerminateDisabilityClaimControls
    {
        public void SelectTerminationReason(string reason)
        {
            base.AvailableReasons.Select(reason);
            base.AddReason.Click();
        }

        public void SubmitDisabilityClaimTermination()
        {
            CommonIEDialogHandler.SelectOK(base.Submit);
            base.Document.DomContainer.WaitForComplete();
        }

        public string SubmitDisabilityClaimTerminationWithoutSelectingAReason()
        {
            var message = CommonIEDialogHandler.SelectOKInAlertDialogAndReturnMessage(base.Submit);
            base.Document.DomContainer.WaitForComplete();
            return message;
        }

        public void CancelDisabilityClaimBeforeTermination()
        {
            base.Cancel.Click();
        }  
    }
}