using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class RecoveriesProposalCaptureControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_gvRecoveriesProposal_DXMainTable")]
        protected Table RecoveriesProposalDetailsTable { get; set; }

        [FindBy(Id = "ctl00_Main_txtShortfallAmount_txtRands")]
        protected TextField ShortfallAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtShortfallAmount_txtCents")]
        protected TextField ShortfallAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtRepaymentAmount_txtRands")]
        protected TextField RepaymentAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtRepaymentAmount_txtCents")]
        protected TextField RepaymentAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_dteStartDate")]
        protected TextField StartDate { get; set; }

        [FindBy(Id = "ctl00_Main_chkAOD")]
        protected CheckBox AcknowledgementOfDebt { get; set; }

        [FindBy(Id = "ctl00_Main_AddButton")]
        protected Button Add { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button Cancel { get; set; }
    }
}