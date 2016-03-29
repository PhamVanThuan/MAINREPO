using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeCorrespondenceLetterOfAcceptanceControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtFaxCode")]
        protected TextField ctl00MaintxtFaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtFax")]
        protected TextField ctl00MaintxtFax { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmail")]
        protected TextField ctl00MaintxtEmail { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClient")]
        protected Span ctl00MainWorkFlowHeader1lblClient { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClientName")]
        protected Span ctl00MainWorkFlowHeader1lblClientName { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoan")]
        protected Span ctl00MainWorkFlowHeader1lblLoan { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoanNumber")]
        protected Span ctl00MainWorkFlowHeader1lblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_btnAddAddress")]
        protected Button ctl00MainbtnAddAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnPreview")]
        protected Button ctl00MainbtnPreview { get; set; }

        [FindBy(Id = "ctl00_Main_btnSend")]
        protected Button ctl00MainbtnSend { get; set; }

        [FindBy(Id = "ctl00_Main_chkFax")]
        protected CheckBox ctl00MainchkFax { get; set; }

        [FindBy(Id = "ctl00_Main_chkEmail")]
        protected CheckBox ctl00MainchkEmail { get; set; }

        [FindBy(Id = "ctl00_Main_chkPost")]
        protected CheckBox ctl00MainchkPost { get; set; }

        [FindBy(Id = "ctl00_Main_chkSMS")]
        protected CheckBox ctl00MainchkSMS { get; set; }
    }
}