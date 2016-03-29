using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeBenefitsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button ctl00MainbtnNext { get; set; }

        [FindBy(Id = "ctl00_Main_ctl00")]
        protected CheckBox ctl00Mainchk00 { get; set; }

        [FindBy(Id = "ctl00_Main_ctl01")]
        protected CheckBox ctl00Mainchk01 { get; set; }
    }
}