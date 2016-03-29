using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CATSDisbursementDeleteControls : CATSDisbursementBaseControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnDelete { get; set; }
    }
}