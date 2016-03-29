using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifePolicyAdminControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button AddLifeButton { get; set; }

        [FindBy(Id = "ctl00_Main_LegalEntityGrid")]
        protected Table AssuredLifeLegalEntities { get; set; }

        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button RemoveLifeButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnRecalculatePremiums")]
        public Button RecalculatePremiumsButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnPremiumQuote")]
        public Button PremiumCalculatorButton { get; set; }
    }
}