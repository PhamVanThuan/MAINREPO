using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    /// <summary>
    /// This screen is used to add/remove sureties from an account
    /// </summary>
    public class RelatedLegalEntitySuretorRemove : RelatedLegalEntitySuretorRemoveControls
    {
        private readonly IWatiNService watinService;

        public RelatedLegalEntitySuretorRemove()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Selects a legal entity from the grid
        /// </summary>
        /// <param name="idNumber"></param>
        public void SelectLegalEntityByIDNumber(string idNumber)
        {
            base.SelectGridRowByIDNumber(idNumber);
        }

        /// <summary>
        /// Clicks the remove button
        /// </summary>
        public void ClickRemove()
        {
            watinService.HandleConfirmationPopup(base.btnRemove);
        }

        /// <summary>
        /// Checks the status of the remove button
        /// </summary>
        /// <param name="enabled"></param>
        public void AssertRemoveButtonEnabled(bool enabled)
        {
            Assert.That(base.btnRemove.Enabled == enabled, "The remove button's enabled state does not match the state provided.");
        }
    }
}