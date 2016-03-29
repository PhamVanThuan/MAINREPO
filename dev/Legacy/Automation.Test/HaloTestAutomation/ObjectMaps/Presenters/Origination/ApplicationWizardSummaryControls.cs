using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationWizardSummaryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnCalculator")]
        protected Button btnUpdateCalculator { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdateApplicant { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button btnNextApplicant { get; set; }

        [FindBy(Id = "ctl00_Main_btnFinish")]
        protected Button btnFinish { get; set; }
    }
}