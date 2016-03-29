using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualDebitOrderAddRecurringControls : ManualDebitOrderBaseControls
    {
        [FindBy(Id = "ctl00_Main_txtNoOfPayments")]
        protected TextField NoOfPayments { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button Add { get; set; }
    }
}