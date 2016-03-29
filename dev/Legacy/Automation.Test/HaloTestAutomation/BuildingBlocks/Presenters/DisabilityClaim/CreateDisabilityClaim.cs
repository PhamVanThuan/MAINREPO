using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.DisabilityClaim
{
    public class CreateDisabilityClaim : DisabilityClaimCreateControls
    {
        public void SelectDisabilityClaimant(int legalEntityKey)
        {
            base.SelectClaimant.SelectByValue(legalEntityKey.ToString());
        }

        public void ClickCreateClaimButton()
        {
            base.CreateClaimButton.Click();
        }
    }
}