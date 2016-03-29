using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class MarkNonPerformingControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnProceed")]
        protected Button Proceed { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_NonPerformingCheck")]
        protected CheckBox MarkNonPerformingCheckBox { get; set; }

        [FindBy(Id = "ctl00_Main_lblSuspendedInterestAmt")]
        protected Span SuspendedInterestAmountVariable { get; set; }

        [FindBy(Id = "ctl00_Main_lblMonthToDateInterestAmt")]
        protected Span MonthToDateInterestAmountVariable { get; set; }

        [FindBy(Id = "ctl00_Main_lblSuspendedInterestFixedAmount")]
        protected Span SuspendedInterestAmountFixed { get; set; }

        [FindBy(Id = "ctl00_Main_lblMonthToDateInterestFixedAmt")]
        protected Span MonthTodateInterestAmountFixed { get; set; }
    }
}