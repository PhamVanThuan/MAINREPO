using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationSummaryFurtherLendingControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ApplicationDetail_content_lbAccountNumber")]
        protected Span lblAccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_ApplicationDetail_content_lblApplicationProcessor")]
        protected Span lblApplicationProcessor { get; set; }
    }
}