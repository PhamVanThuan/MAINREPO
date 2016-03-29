using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class WorkflowYesNoControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnYes")]
        protected Button btnYes { get; set; }

        [FindBy(Id = "ctl00_Main_btnNo")]
        protected Button btnNo { get; set; }
    }
}