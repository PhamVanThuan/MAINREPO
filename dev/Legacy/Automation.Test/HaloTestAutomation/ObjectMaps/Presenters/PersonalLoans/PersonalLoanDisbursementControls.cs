using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanDisbursementControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnConfirm")]
        protected Button btnConfirm { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_lblDisbursementAmount")]
        protected Span lblDisbursementAmount { get; set; }

        [FindBy(Id = "ctl00_Main_lblBank")]
        protected Span lblBank { get; set; }

        [FindBy(Id = "ctl00_Main_lblBranch")]
        protected Span lblBranch { get; set; }

        [FindBy(Id = "ctl00_Main_lblAccountType")]
        protected Span lblAccountType { get; set; }

        [FindBy(Id = "ctl00_Main_lblAccountName")]
        protected Span lblAccountName { get; set; }

        [FindBy(Id = "ctl00_Main_lblAccountNumber")]
        protected Span lblAccountNumber { get; set; }
    }
}