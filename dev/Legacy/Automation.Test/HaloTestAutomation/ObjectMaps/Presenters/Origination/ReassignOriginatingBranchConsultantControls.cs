using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ReassignOriginatingBranchConsultantControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtMemo")]
        protected TextField txtMemo { get; set; }

        [FindBy(Id = "ctl00_Main_ddlConsultant")]
        protected SelectList ddlConsultant { get; set; }

        [FindBy(Name = "ctl00$Main$chkReassignBC")]
        protected CheckBox chkReassignBC { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }
    }
}