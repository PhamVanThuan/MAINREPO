using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeLOAControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtRegistrationAmount")]
        protected TextField ctl00MaintxtRegistrationAmount { get; set; }

        [FindBy(Id = "ctl00_Main_txtLoanAgreementAmount")]
        protected TextField ctl00MaintxtLoanAgreementAmount { get; set; }

        [FindBy(Id = "ctl00_Main_txtCashPortion")]
        protected TextField ctl00MaintxtCashPortion { get; set; }

        [FindBy(Id = "ctl00_Main_txtInterimInterest")]
        protected TextField ctl00MaintxtInterimInterest { get; set; }

        [FindBy(Id = "ctl00_Main_txtMonthlyInstalment")]
        protected TextField ctl00MaintxtMonthlyInstalment { get; set; }

        [FindBy(Id = "ctl00_Main_txtVariable")]
        protected TextField ctl00MaintxtVariable { get; set; }

        [FindBy(Id = "ctl00_Main_txtFixed")]
        protected TextField ctl00MaintxtFixed { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCSumAssured")]
        protected TextField ctl00MaintxtHOCSumAssured { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCPremium")]
        protected TextField ctl00MaintxtHOCPremium { get; set; }

        [FindBy(Id = "ctl00_Main_txtTotalMonthlyInstalment")]
        protected TextField ctl00MaintxtTotalMonthlyInstalment { get; set; }

        [FindBy(Id = "ctl00_Main_txtBondTerm")]
        protected TextField ctl00MaintxtBondTerm { get; set; }

        [FindBy(Id = "ctl00_Main_txtVariFix")]
        protected TextField ctl00MaintxtVariFix { get; set; }

        [FindBy(Id = "ctl00_Main_txtInterestOnly")]
        protected TextField ctl00MaintxtInterestOnly { get; set; }

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

        [FindBy(Id = "ctl00_Main_lblAttorneyName")]
        protected Span ctl00MainlblAttorneyName { get; set; }

        [FindBy(Id = "ctl00_Main_lblLoanConsultant")]
        protected Span ctl00MainlblLoanConsultant { get; set; }

        [FindBy(Id = "ctl00_Main_SAHLLabel2")]
        protected Span ctl00MainSAHLLabel2 { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button ctl00MainbtnNext { get; set; }
    }
}