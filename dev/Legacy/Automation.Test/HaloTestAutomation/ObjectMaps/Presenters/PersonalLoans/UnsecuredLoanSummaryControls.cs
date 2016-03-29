using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class UnsecuredLoanSummaryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_pendingLifePolicyClaimHeading")]
        protected Div PendingLifePolicyClaimHeading { get; set; }
    }
}