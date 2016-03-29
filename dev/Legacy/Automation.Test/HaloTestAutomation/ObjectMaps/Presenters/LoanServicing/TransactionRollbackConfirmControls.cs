using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.LoanServicing
{
    public abstract class TransactionRollbackConfirmControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnRollbackConfirm")]
        protected Button RollbackConfirm { get; set; }
    }
}