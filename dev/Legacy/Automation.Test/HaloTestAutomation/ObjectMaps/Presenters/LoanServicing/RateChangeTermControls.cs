using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class RateChangeTermControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_CalcNewTerm")]
        protected Button CalculateButton { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button ProcessTermChangeButton { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_NewRemainingTerm")]
        protected TextField NewRemainingTerm { get; set; }

        [FindBy(Id = "ctl00_Main_TermMemoComments")]
        protected TextField Comments { get; set; }
    }
}