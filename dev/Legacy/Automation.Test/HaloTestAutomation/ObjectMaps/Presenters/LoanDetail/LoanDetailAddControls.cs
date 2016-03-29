using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LoanDetailAddControls : LoanDetailBaseControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Submit { get; set; }
    }
}