using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeNTUControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClient")]
        protected Span lblClient { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClientName")]
        protected Span lblClientName { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblPolicyTypeDesc")]
        protected Span lblPolicyTypeDesc { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblPolicyType")]
        protected Span lblPolicyType { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoan")]
        protected Span lblLoan { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoanNumber")]
        protected Span lblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_Label1")]
        protected Span ctl00MainLabel1 { get; set; }

        [FindBy(Id = "ctl00_Main_lblReason")]
        protected Span lblReason { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNTUReason")]
        protected SelectList ddlNTUReason { get; set; }
    }
}