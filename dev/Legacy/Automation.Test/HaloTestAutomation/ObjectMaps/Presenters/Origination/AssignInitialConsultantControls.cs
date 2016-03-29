using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AssignInitialConsultantControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlConsultant")]
        public SelectList selectUser { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        public Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        public Button btnCancel { get; set; }
    }
}