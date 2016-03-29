using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicantsRemoveControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button btnRemove { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_grdApplicants")]
        protected Table ApplicantsGrid { get; set; }

        protected TableRow gridSelectOffer(string CellValue)
        {
            TableCellCollection applicants = ApplicantsGrid.TableCells;
            return applicants.Filter(Find.ByText(CellValue))[0].ContainingTableRow;
        }
    }
}