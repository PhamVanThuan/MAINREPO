using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeCallbackControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dteCallbackDate")]
        protected TextField ctl00MaindteCallbackDate { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button ctl00MainbtnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button ctl00MainbtnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlReason")]
        protected SelectList ctl00MainddlReason { get; set; }

        [FindBy(Id = "ctl00_Main_ddlHour")]
        protected SelectList ctl00MainddlHour { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMin")]
        protected SelectList ctl00MainddlMin { get; set; }

        [FindBy(Id = "ctl00_Main_txtNotes")]
        protected TextField ctl00MaintxtNotes { get; set; }
    }
}