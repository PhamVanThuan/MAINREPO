using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class LifeProductSwitchControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClient")]
        protected Span ctl00MainWorkFlowHeader1lblClient { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClientName")]
        protected Span ctl00MainWorkFlowHeader1lblClientName { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblPolicyTypeDesc")]
        protected Span ctl00MainWorkFlowHeader1lblPolicyTypeDesc { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblPolicyType")]
        protected Span ctl00MainWorkFlowHeader1lblPolicyType { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoan")]
        protected Span ctl00MainWorkFlowHeader1lblLoan { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoanNumber")]
        protected Span ctl00MainWorkFlowHeader1lblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_Label1")]
        protected Span ctl00MainLabel1 { get; set; }

        [FindBy(Id = "ctl00_Main_lblReason")]
        protected Span ctl00MainlblReason { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button ctl00MainbtnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlPolicyType")]
        protected SelectList ctl00MainddlPolicyType { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button ctl00MainbtnSubmit { get; set; }
    }
}