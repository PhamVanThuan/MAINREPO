using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.Admin
{
    public abstract class MarketingSourceAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtMarketingSourceDescription")]
        protected TextField txtMarketingSourceDescription { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMarketingSourceStatus")]
        protected SelectList ddlMarketingSourceStatus { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_gvMarketingSources")]
        protected Table tblMarketingSources { get; set; }

        protected TableRow gridSelectMarketingSource(string cellValue)
        {
            TableCellCollection marketingSources = tblMarketingSources.TableCells;
            return marketingSources.Filter(Find.ByText(cellValue))[0].ContainingTableRow;
        }

        [FindBy(Id = "ctl00_Main_valStatus")]
        protected Span lblValidation { get; set; }
    }
}