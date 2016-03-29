using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FurtherLendingApplicationApproveControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnApprove { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnQuickCash")]
        protected Button btnQuickCash { get; set; }
    }
}