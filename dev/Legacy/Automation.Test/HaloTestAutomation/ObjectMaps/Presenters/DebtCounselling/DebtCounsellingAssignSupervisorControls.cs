using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebtCounsellingAssignSupervisorControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlUser")]
        protected SelectList UserDropDown { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button SubmitButton { get; set; }
    }
}