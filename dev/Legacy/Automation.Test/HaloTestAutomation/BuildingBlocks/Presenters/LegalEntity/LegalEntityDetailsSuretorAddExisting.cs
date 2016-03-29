using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityDetailsSuretorAddExisting : LegalEntitySuretorAddExistingControls
    {
        /// <summary>
        /// clicks the Add Suretor button
        /// </summary>
        public void AddSuretor()
        {
            base.AddSuretorButton.Click();
        }
    }
}