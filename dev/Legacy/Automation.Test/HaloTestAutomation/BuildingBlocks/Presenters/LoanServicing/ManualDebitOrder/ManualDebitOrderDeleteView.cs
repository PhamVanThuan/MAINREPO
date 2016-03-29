using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders
{
    public sealed class ManualDebitOrderDeleteView : ManualDebitOrderDeleteControls
    {
        private readonly IWatiNService watinService;

        public ManualDebitOrderDeleteView()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Click Delete
        /// </summary>
        /// <param name="createDeleteRequest"></param>
        public void ClickDelete(bool createDeleteRequest = false)
        {
            if (createDeleteRequest)
            {
                base.Delete.Click();
                base.divValidationSummaryBody.Buttons[0].Click();
            }
            else
            {
                watinService.HandleConfirmationPopup(base.Delete);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ClickCancel()
        {
            base.Cancel.Click();
        }
    }
}