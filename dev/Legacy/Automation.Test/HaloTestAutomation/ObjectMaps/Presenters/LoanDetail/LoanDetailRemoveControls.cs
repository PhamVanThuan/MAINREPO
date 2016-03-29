using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LoanDetailDeleteControls : LoanDetailBaseControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Delete { get; set; }
    }
}