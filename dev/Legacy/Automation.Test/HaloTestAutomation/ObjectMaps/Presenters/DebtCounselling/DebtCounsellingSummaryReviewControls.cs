using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebtCounsellingSummaryReviewControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblEWorkWarningMessage")]
        protected Span lblEWorkMessage { get; set; }

        [FindBy(Text = "E-Work Loss Control Case Details")]
        protected Link EworkCaseDetails { get; set; }

        [FindBy(Id = "ctl00_Main_apEWorkDetails_content_lblEworkStage")]
        protected Span lblEworkStage { get; set; }

        [FindBy(Id = "ctl00_Main_apEWorkDetails_content_lblEworkUser")]
        protected Span lblEworkUser { get; set; }

        [FindBy(Text = "Debt Counselling Details")]
        protected Link DebtCounsellingDetails { get; set; }

        [FindBy(Id = "ctl00_Main_apDebtCounsellingDetails_content_lblRemainingTerm")]
        protected Span lblRemainingTerm { get; set; }

        [FindBy(Id = "ctl00_Main_apDebtCounsellingDetails_content_lblRemainingTermHighlight")]
        protected Span lblRemainingTermHighlight { get; set; }
    }
}