using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public class DisabilityClaimNodeControls : BaseNavigation
    {
        [FindBy(Title = "Capture Details")]
        protected Link ClickCaptureDetails { get; set; }

        [FindBy(Title = "Repudiate")]
        protected Link ClickRepudiate { get; set; }

        [FindBy(Title = "Approve")]
        protected Link ClickApprove { get; set; }

        [FindBy(Text = "Send Approval Letter")]
        protected Link ClickSendApprovalLetter { get; set; }

        [FindBy(Title = "Terminate")]
        protected Link ClickTerminate { get; set; }
    }
}