using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DisabilityClaimCreateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button CreateClaimButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlClaimant")]
        protected SelectList SelectClaimant { get; set; }
    }
}