using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Cap
{
    public class CAPCreateSearch : CAPCreateSearchControls
    {
        private readonly IWatiNService watinService;

        public CAPCreateSearch()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Creates a CAP 2 Offer from HALO
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="AccountKey">Mortgage Loan Account to create the offer against</param>
        public void CreateCAP2Offer(int AccountKey)
        {
            base.txtAccountNumber.TypeText(AccountKey.ToString());
            base.SearchButton.Click();
            base.SubmitButton.Click();
            watinService.HandleConfirmationPopup(base.SubmitButton);
        }

        /// <summary>
        /// This is used for the tests that require to check if domain warning messages are being returned to the screen.
        /// The messages are returned after 2 different button clicks so this method will carry on creating the case until
        /// the domain validation warning exists
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="AccountKey">AccountKey</param>
        public void CreateCAP2OfferCheckWarnings(int AccountKey)
        {
            base.txtAccountNumber.TypeText(AccountKey.ToString());
            base.SearchButton.Click();
            base.SubmitButton.Click();
            if (!base.divValidationSummaryBody.Exists)
            {
                watinService.HandleConfirmationPopup(base.SubmitButton);
            }
        }
    }
}