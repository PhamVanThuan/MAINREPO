using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationLoanDetailsApproveControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlSPV")]
        protected SelectList ddlSPV { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchCashOut_txtRands")]
        protected TextField txtSwitchCashOut_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchCashOut_txtCents")]
        protected TextField txtSwitchCashOut_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtRefinanceCashOut_txtRands")]
        protected TextField txtRefinanceCashOut_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtRefinanceCashOut_txtCents")]
        protected TextField txtRefinanceCashOut_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtNewPurchaseCashDeposit_txtRands")]
        protected TextField txtNewPurchaseCashDeposit_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtNewPurchaseCashDeposit_txtCents")]
        protected TextField txtNewPurchaseCashDeposit_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_chkDiscountedLinkRate")]
        protected CheckBox chkDiscountedLinkRate { get; set; }

        [FindBy(IdRegex = "^[\x20-\x7E]*txtDiscount$")]
        protected TextField txtDiscount { get; set; }

        [FindBy(IdRegex = "^[\x20-\x7E]*txtDiscount_bUp$")]
        protected Button btnDiscount_bUp { get; set; }

        [FindBy(IdRegex = "^[\x20-\x7E]*txtDiscount_bDown$")]
        protected Button btnDiscount_bDown { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtTotalAmountApproved_txtRands")]
        protected TextField txtQuickCashDetails__txtTotalAmountApproved_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtTotalAmountApproved_txtCents")]
        protected TextField txtQuickCashDetails__txtTotalAmountApproved_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtUpfrontPaymentApproved_txtRands")]
        protected TextField txtQuickCashDetails__txtUpfrontPaymentApproved_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtUpfrontPaymentApproved_txtCents")]
        protected TextField txtQuickCashDetails__txtUpfrontPaymentApproved_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails_ctl17")]
        protected Button btnQuickCashDetails_ctl17 { get; set; }

        [FindBy(Id = "ctl00_Main_btnRecalc")]
        protected Button btnRecalc { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtCashPortion")]
        protected Span spanQuickCashDetails__txtCashPortion { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtMaximumQuickCash")]
        protected Span spanQuickCashDetails__txtMaximumQuickCash { get; set; }

        [FindBy(Id = "ctl00_Main_chkQuickPayFee")]		
        protected CheckBox chkQuickPayLoan { get; set; }
    }
}