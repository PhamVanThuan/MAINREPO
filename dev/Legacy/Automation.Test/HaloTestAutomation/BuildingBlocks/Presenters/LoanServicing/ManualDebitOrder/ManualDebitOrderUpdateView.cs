using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders
{
    public sealed class ManualDebitOrderUpdateView : ManualDebitOrderUpdateControls
    {
        private readonly IWatiNService watinService;

        public ManualDebitOrderUpdateView()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ClickUpdate(bool handlePopup)
        {
            if (handlePopup)
            {
                watinService.HandleConfirmationPopup(base.Update);
            }
            else
            {
                base.Update.Click();
            }
        }

        public void ClickCancel()
        {
            base.Cancel.Click();
        }

        /// <summary>
        /// Updates the amount on the manual debit order
        /// </summary>
        /// <param name="newAmount"></param>
        public void UpdateManualDebitOrderAmount(int newAmount, string note, string reference, bool handlePopup)
        {
            base.AmountRands.Clear();
            base.AmountRands.TypeText(newAmount.ToString());
            base.Note.Value = note;
            base.Reference.Value = reference;
            ClickUpdate(handlePopup);
        }
    }
}