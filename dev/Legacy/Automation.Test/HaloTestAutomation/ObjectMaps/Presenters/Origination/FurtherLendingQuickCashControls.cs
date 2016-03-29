using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FurtherLendingQuickCashControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSave")]
        protected Button btnSave { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails_ctl17")]
        protected Button btnQCDeclineReasons { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtTotalAmountApproved_txtRands")]
        protected TextField txtTotalApprovedRands { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtTotalAmountApproved_txtCents")]
        protected TextField txtTotalApprovedCents { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtUpfrontPaymentApproved_txtRands")]
        protected TextField txtTotalUpfrontApprovedRands { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtUpfrontPaymentApproved_txtCents")]
        protected TextField txtTotalUpfrontApprovedCents { get; set; }

        [FindBy(Id = "ctl00_Main_QuickCashDetails__txtMaximumQuickCash")]
        protected Span lblMaximumQC { get; set; }
    }
}