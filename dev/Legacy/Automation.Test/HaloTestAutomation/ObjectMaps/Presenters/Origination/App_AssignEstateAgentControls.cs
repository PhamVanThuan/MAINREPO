using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class App_AssignEstateAgentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlConsultant")]
        protected SelectList ddlConsultant { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }
    }
}