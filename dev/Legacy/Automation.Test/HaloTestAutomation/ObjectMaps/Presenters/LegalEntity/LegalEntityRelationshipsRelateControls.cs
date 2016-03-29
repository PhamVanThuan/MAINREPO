using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LegalEntityRelationshipsRelateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlRelationshipType")]
        protected SelectList ddlRelationshipType { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_grdRelatedLegalEntities")]
        protected Table grdRelatedLegalEntities { get; set; }

        public TableRow gridSelectRelationship(string CellValue)
        {
            TableCellCollection relationships = grdRelatedLegalEntities.TableCells;
            return relationships.Filter(Find.ByText(CellValue))[0].ContainingTableRow;
        }
    }
}