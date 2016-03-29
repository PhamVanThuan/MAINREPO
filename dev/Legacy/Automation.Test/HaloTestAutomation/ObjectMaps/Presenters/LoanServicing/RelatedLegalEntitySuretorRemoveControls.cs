using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class RelatedLegalEntitySuretorRemoveControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_RelatedLEGrid")]
        protected Table LegalEntityGrid { get; set; }

        /// <summary>
        /// Selects the grid row given the id number
        /// </summary>
        /// <param name="idNumber"></param>
        protected void SelectGridRowByIDNumber(string idNumber)
        {
            TableCellCollection roles = LegalEntityGrid.TableCells;
            var row = roles.Filter(Find.ByText(idNumber))[0].ContainingTableRow;
            row.Click();
        }

        [FindBy(Id = "ctl00_Main_RemoveButton")]
        protected Button btnRemove { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }
    }
}