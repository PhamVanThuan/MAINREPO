using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class LifeRPARControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClient")]
        protected Span ctl00MainWorkFlowHeader1lblClient { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClientName")]
        protected Span ctl00MainWorkFlowHeader1lblClientName { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoan")]
        protected Span ctl00MainWorkFlowHeader1lblLoan { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoanNumber")]
        protected Span ctl00MainWorkFlowHeader1lblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_Label1")]
        protected Span ctl00MainLabel1 { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button ctl00MainbtnNext { get; set; }

        [FindBy(Id = "ctl00_Main_rblYesNo_0")]
        protected RadioButton ctl00MainrblYesNo0 { get; set; }

        [FindBy(Id = "ctl00_Main_rblYesNo_1")]
        protected RadioButton ctl00MainrblYesNo1 { get; set; }
    }
}