using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LoanSummaryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblSPVDescription")]
        protected Span SPVDescription { get; set; }
    }
}