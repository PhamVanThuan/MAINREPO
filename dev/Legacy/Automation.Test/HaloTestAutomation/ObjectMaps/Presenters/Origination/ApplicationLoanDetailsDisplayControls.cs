using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationLoanDetailsDisplayControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblPropertyValue")]
        protected Span lblPropertyValue { get; set; }

        [FindBy(Id = "ctl00_Main_lblSwitchExistingLoan")]
        protected Span lblSwitchExistingLoan { get; set; }

        [FindBy(Id = "ctl00_Main_lblSwitchCashOut")]
        protected Span lblSwitchCashOut { get; set; }

        [FindBy(Id = "ctl00_Main_lblLTV")]
        protected Span lblLTV { get; set; }
    }
}