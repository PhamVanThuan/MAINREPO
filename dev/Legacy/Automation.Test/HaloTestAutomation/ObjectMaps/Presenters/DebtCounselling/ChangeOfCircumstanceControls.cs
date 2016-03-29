using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ChangeOfCircumstanceControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSave")]
        protected Button btnSave { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_dte17point3")]
        protected TextField txt173Date { get; set; }

        [FindBy(Id = "ctl00_Main_txtComments")]
        protected TextField txtComments { get; set; }
    }
}