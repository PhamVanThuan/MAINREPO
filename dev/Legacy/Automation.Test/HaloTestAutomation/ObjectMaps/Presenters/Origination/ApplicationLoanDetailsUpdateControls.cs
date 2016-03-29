using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationLoanDetailsUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtBondtoRegister")]
        protected TextField BondtoRegister { get; set; }

        [FindBy(Id = "ctl00_Main_txtBondtoRegister_txtRands")]
        protected TextField BondToRegisterRandValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtBondtoRegister_txtCents")]
        protected TextField BondToRegisterCentsValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtPropertyValue")]
        protected TextField EstPropertyValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtTerm")]
        protected TextField Term { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchExistingLoan")]
        protected TextField SwitchExistingLoan { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchExistingLoan_txtRands")]
        protected TextField SwitchExistingLoanRandValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchExistingLoan_txtCents")]
        protected TextField SwitchExistingLoanCentsValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchCashOut")]
        protected TextField SwitchCashOut { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchCashOut_txtRands")]
        protected TextField SwitchCashOutRandValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtSwitchCashOut_txtCents")]
        protected TextField SwitchCashOutCentsValue { get; set; }

        protected TextField DiscountSurcharge
        {
            get
            {
                return base.Document.TextField(Find.ById(new Regex(@"^[\x20-\x7E]*txtDiscount$")));
            }
        }

        [FindBy(Id = "ctl00_Main_txtCancellationFee_txtRands")]
        protected TextField CancellationFeeRandValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtCancellationFee_txtCents")]
        protected TextField CancellationFeeCentsValue { get; set; }

        [FindBy(Id = "ctl00_Main_VarifixDetails__txtFixedPercentage_txtRands")]
        protected TextField VariFixFixedPercentage { get; set; }

        [FindBy(Id = "ctl00_Main_btnRecalc")]
        protected Button RecalculateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_VarifixDetails__btnUseMaximum")]
        protected Button UseMaximumVariFix { get; set; }

        [FindBy(Id = "ctl00_Main_ddlProduct")]
        protected SelectList ProductDropdown { get; set; }

        [FindBy(Name = "ctl00$Main$chkCapitaliseFees")]
        protected CheckBox CapitaliseFeesAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkOverrideFees")]
        protected CheckBox OverrideFeesAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkQuickCash")]
        protected CheckBox QuickCashAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkHOC")]
        protected CheckBox HOCAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkStaffLoan")]
        protected CheckBox StaffLoanAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkLife")]
        protected CheckBox LifeAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkCAP")]
        protected CheckBox CAPAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkInterestOnly")]
        protected CheckBox InterestOnlyAttribute { get; set; }

        [FindBy(Name = "ctl00$Main$chkDiscountedLinkRate")]
        protected CheckBox DiscountedLinkRateAttribute { get; set; }

        [FindBy(Id = "ctl00_Main_lblSPVName")]
        protected Span SPVSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblApplicationType")]
        protected Span ApplicationTypeSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblCategory")]
        protected Span CategorySpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblMinimumBond")]
        protected Span MinimumBondSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblHouseHoldIncome")]
        protected Span HouseHoldIncomeSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblSwitchInterimInterest")]
        protected Span InterimInterestSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblSwitchFees")]
        protected Span SwitchFeesSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblSwitchTotalLoanRequired")]
        protected Span SwitchTotalLoanRequiredSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblCancellationFee")]
        protected Span CancellationFeeSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblInitiationFee")]
        protected Span InitiationFeeSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblRegistrationFee")]
        protected Span RegistrationFeeSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblLTV")]
        protected Span LTVSpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblPTI")]
        protected Span PTISpan { get; set; }

        [FindBy(Id = "ctl00_Main_lblPTIVF")]
        protected Span VariFixPTISpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblMarketRatePerc")]
        protected Span MarketRateSpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblLinkRate")]
        protected Span LinkRateSpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblDiscount")]
        protected Span DiscountSpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblPricingAdjustment")]
        protected Span PricingAdjustmentSpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblEffectiveRate")]
        protected Span EffectiveRateSpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblAmortisingInstalment")]
        protected Span AmortisingInstalmentSpan { get; set; }

        [FindBy(Id = "ctl00_Main_LoanDetails_lblInterestOnlyInstallment")]
        protected Span InterestOnlyInstalmentSpan { get; set; }
        
        [FindBy(Id = "ctl00_Main_txtRefinanceCashOut_txtRands")]
        protected TextField RefinanceCashOutRandValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtRefinanceCashOut_txtCents")]
        protected TextField RefinanceCashOutCentValue { get; set; }

        [FindBy(Id = "ctl00_Main_chkStaffLoan")]
        protected CheckBox StaffLoan { get; set; }

        [FindBy(Id = "ctl00_Main_chkCapitaliseInitiationFees")]
        protected CheckBox CapitaliseInitiationFees { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalLoanRequired")]
        protected Span TotalLoanRequired { get; set; }

        protected Button MainDiscountUp
        {
            get
            {
                return base.Document.Button(Find.ById(new Regex(@"^[\x20-\x7E]*txtDiscount_bUp$")));
            }
        }

        protected Button MainDiscountDown
        {
            get
            {
                return base.Document.Button(Find.ById(new Regex(@"^[\x20-\x7E]*txtDiscount_bDown$")));
            }
        }

    }
}