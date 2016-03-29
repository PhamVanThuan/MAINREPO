using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class InitiateCaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dte17pt1")]
        protected TextField _17Pt1Date { get; set; }

        [FindBy(Id = "ctl00_Main_txtComments")]
        protected TextField Comments { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button Next { get; set; }
    }
}