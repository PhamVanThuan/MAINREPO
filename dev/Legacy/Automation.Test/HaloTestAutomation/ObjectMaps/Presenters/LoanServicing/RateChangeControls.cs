using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class RateChangeControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_NewLinkRate")]
        protected SelectList NewLinkRate { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button ChangeRate { get; set; }

        [FindBy(Id = "ctl00_Main_RateMarketRateVariable")]
        protected Span MarketRateVariable { get; set; }

        [FindBy(Id = "ctl00_Main_RateDiscountVariable")]
        protected Span DiscountVariable { get; set; }

        [FindBy(Id = "ctl00_Main_RateNewIntRateVariable")]
        protected Span NewInterestRateVariable { get; set; }

        [FindBy(Id = "ctl00_Main_RateNewInstallmentVariable")]
        protected Span NewInstalmentVariable { get; set; }

        [FindBy(Id = "ctl00_Main_RateMarketRateFixed")]
        protected Span MarketRateFixed { get; set; }

        [FindBy(Id = "ctl00_Main_RateDiscountFixed")]
        protected Span DiscountFixed { get; set; }

        [FindBy(Id = "ctl00_Main_RateNewIntRateFixed")]
        protected Span NewInterestRateFixed { get; set; }

        [FindBy(Id = "ctl00_Main_RateNewInstallmentFixed")]
        protected Span NewInstalmentFixed { get; set; }
    }
}