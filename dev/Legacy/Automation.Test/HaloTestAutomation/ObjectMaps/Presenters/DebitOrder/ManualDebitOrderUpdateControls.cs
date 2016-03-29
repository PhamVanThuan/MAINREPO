using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualDebitOrderUpdateControls : ManualDebitOrderBaseControls
    {
        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button Update { get; set; }
    }
}