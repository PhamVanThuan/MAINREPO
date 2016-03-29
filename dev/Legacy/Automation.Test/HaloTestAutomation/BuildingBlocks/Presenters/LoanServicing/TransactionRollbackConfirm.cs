using ObjectMaps.Presenters.LoanServicing;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class TransactionRollbackConfirm : TransactionRollbackConfirmControls
    {
        public void ConfirmRollBack()
        {
            base.RollbackConfirm.Click();
        }
    }
}