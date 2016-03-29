using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class WF_ReAssignControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlRole")]
        protected SelectList ddlRole { get; set; }

        [FindBy(Id = "ctl00_Main_ddlConsultant")]
        protected SelectList ddlConsultant { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }
    }
}