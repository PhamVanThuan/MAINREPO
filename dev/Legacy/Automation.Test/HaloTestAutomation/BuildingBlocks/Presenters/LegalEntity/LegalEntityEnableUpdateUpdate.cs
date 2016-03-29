using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityEnableUpdateUpdate : EnableLegalEntityUpdateControls
    {
        public void ClickYes()
        {
            base.btnYes.Click();
        }

        public void ClickNo()
        {
            base.btnNo.Click();
        }
    }
}