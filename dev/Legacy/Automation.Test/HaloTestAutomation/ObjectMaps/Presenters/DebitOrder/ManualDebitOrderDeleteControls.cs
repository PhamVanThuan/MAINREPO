using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualDebitOrderDeleteControls : ManualDebitOrderBaseControls
    {
        [FindBy(Id = "ctl00_Main_btnDelete")]
        protected Button Delete { get; set; }
    }
}