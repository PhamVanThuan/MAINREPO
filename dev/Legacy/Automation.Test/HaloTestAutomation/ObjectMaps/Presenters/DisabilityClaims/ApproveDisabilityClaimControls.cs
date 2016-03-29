using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApproveDisabilityClaimControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtInstalmentsAuthorised")]
        protected TextField NumberOfInstalmentsAuthorised { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button Submit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_lblExpectedReturnToWorkDate")]
        protected Label ExpectedReturnToWorkDate { get; set; }
    }
}