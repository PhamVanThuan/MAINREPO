using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class MarketRatesViewControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_MarketRateGrid")]
        protected Table MarketRatesGrid { get; set; }

        protected TableRow gridSelectMarketRate(string CellValue)
        {
            TableCellCollection marketRates = MarketRatesGrid.TableCells;
            return marketRates.Filter(Find.ByText(CellValue))[0].ContainingTableRow;
        }

        [FindBy(Id = "ctl00_Main_lblMarketRateValue")]
        protected Span lblMarketRateValue { get; set; }

        [FindBy(Id = "ctl00_Main_lblMarketRateDescription")]
        protected Span lblMarketRateDescription { get; set; }

        [FindBy(Id = "ctl00_Main_txtMarketRateValue")]
        protected TextField MarketRateValue { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Update { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button Cancel { get; set; }
    }
}