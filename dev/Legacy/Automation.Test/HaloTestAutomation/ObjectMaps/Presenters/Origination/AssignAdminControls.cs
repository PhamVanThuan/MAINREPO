using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AssignAdminControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlConsultant")]
        protected SelectList ConsultantDropDown { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button SubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }
    }
}