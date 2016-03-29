using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LoanDetailUpdateControls : LoanDetailBaseControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Update { get; set; }
    }
}