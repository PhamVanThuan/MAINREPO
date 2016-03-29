using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualDebitOrderAddControls : ManualDebitOrderBaseControls
    {
        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button Add { get; set; }
    }
}