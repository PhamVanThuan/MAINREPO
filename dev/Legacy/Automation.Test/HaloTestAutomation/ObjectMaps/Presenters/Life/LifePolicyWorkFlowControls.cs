using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifePolicyWorkFlowControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClient")]
        protected Span lblClient { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClientName")]
        protected Span lblClientName { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoan")]
        protected Span lblLoan { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoanNumber")]
        protected Span WorflowHeaderlblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblPolicyNumber")]
        protected Span lblPolicyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblPolicyStatus")]
        protected Span lblPolicyStatus { get; set; }

        [FindBy(Id = "ctl00_Main_lblPolicyStatusKey")]
        protected Span lblPolicyStatusKey { get; set; }

        [FindBy(Id = "ctl00_Main_lblApplicationStatus")]
        protected Span lblApplicationStatus { get; set; }

        [FindBy(Id = "ctl00_Main_lblConsultant")]
        protected Span lblConsultant { get; set; }

        [FindBy(Id = "ctl00_Main_lblDateOfAcceptance")]
        protected Span lblDateOfAcceptance { get; set; }

        [FindBy(Id = "ctl00_Main_lblDateOfCommencement")]
        protected Span lblDateOfCommencement { get; set; }

        [FindBy(Id = "ctl00_Main_lblCancellationDate")]
        protected Span lblCancellationDate { get; set; }

        [FindBy(Id = "ctl00_Main_lblLoanNumber")]
        protected Span lblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblLoanStatus")]
        protected Span lblLoanStatus { get; set; }

        [FindBy(Id = "ctl00_Main_lblLoanTerm")]
        protected Span lblLoanTerm { get; set; }

        [FindBy(Id = "ctl00_Main_lblLoanAmount")]
        protected Span lblLoanAmount { get; set; }

        [FindBy(Id = "ctl00_Main_lblPipelineStatus")]
        protected Span lblPipelineStatus { get; set; }

        [FindBy(Id = "ctl00_Main_lblLoanDebitOrderDay")]
        protected Span lblLoanDebitOrderDay { get; set; }

        [FindBy(Id = "ctl00_Main_lblLifeCondition")]
        protected Span lblLifeCondition { get; set; }

        [FindBy(Id = "ctl00_Main_lblInitialSumAssured")]
        protected Span lblInitialSumAssured { get; set; }

        [FindBy(Id = "ctl00_Main_lblCurrentSumAssured")]
        protected Span lblCurrentSumAssured { get; set; }

        [FindBy(Id = "ctl00_Main_lblIPBenefitPremium")]
        protected Span blIPBenefitPremium { get; set; }

        [FindBy(Id = "ctl00_Main_lblDeathBenefitPremium")]
        protected Span lblDeathBenefitPremium { get; set; }

        [FindBy(Id = "ctl00_Main_lblAnnualPremium")]
        protected Span lblAnnualPremium { get; set; }

        [FindBy(Id = "ctl00_Main_lblMonthlyInstalment")]
        protected Span lblMonthlyInstalment { get; set; }

        [FindBy(Id = "ctl00_Main_lblBondInstallment")]
        protected Span lblBondInstallment { get; set; }

        [FindBy(Id = "ctl00_Main_lblHOCInstallment")]
        protected Span lblHOCInstallment { get; set; }

        [FindBy(Id = "ctl00_Main_lblMonthlyServiceFee")]
        protected Span lblMonthlyServiceFee { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalAmountDue")]
        protected Span lblTotalAmountDue { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button MainbtnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button btnRemove { get; set; }

        [FindBy(Id = "ctl00_Main_btnRecalculatePremiums")]
        protected Button btnRecalculatePremiums { get; set; }

        [FindBy(Id = "ctl00_Main_btnPremiumQuote")]
        protected Button btnPremiumQuote { get; set; }

        [FindBy(Id = "ctl00_Main_btnAccept")]
        protected Button btnAccept { get; set; }

        [FindBy(Id = "ctl00_Main_btnDecline")]
        protected Button btnDecline { get; set; }

        [FindBy(Id = "ctl00_Main_btnQuote")]
        protected Button btnQuote { get; set; }

        [FindBy(Id = "ctl00_Main_btnConsidering")]
        protected Button btnConsidering { get; set; }

        [FindBy(Id = "ctl00_Main_ddlPolicyHolder")]
        protected SelectList ddlPolicyHolder { get; set; }

        [FindBy(Id = "ctl00_Main_LegalEntityGrid")]
        protected Table MainLegalEntityGrid { get; set; }
    }
}