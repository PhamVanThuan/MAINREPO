using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders
{
    public sealed class ManualDebitOrderAddRecurringView : ManualDebitOrderAddRecurringControls
    {
        public void AddNumberOfPayments(int payments)
        {
            base.NoOfPayments.Value = payments.ToString();
        }

        public void ClickAdd()
        {
            base.Add.Click();
        }

        public void ClickCancel()
        {
            base.Cancel.Click();
        }
    }
}