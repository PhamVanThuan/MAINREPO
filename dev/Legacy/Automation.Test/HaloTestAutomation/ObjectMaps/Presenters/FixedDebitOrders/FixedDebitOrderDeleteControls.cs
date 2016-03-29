using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FixedDebitOrderDeleteControls : FixedDebitOrderBaseControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnDelete { get; set; }
    }
}