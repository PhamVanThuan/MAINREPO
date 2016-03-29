using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class WorkflowBatchReassignControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlRoleType")]
        protected SelectList ddlRoleType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlSearchUser")]
        protected SelectList ddlSearchUser { get; set; }

        [FindBy(Id = "ctl00_Main_btnSearch")]
        protected Button btnSearch { get; set; }

        [FindBy(Id = "ctl00_Main_ddlReassignUser")]
        protected SelectList ddlReassignUser { get; set; }

        [FindBy(Id = "ctl00_Main_btnReassignLeads")]
        protected Button btnReassignLeads { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_SearchGrid")]
        protected Table SearchGrid { get; set; }

        protected TableRowCollection SearchGridRows
        {
            get
            {
                return SearchGrid.TableRows;
            }
        }

        protected TableCellCollection SearchGridRowCells(int RowIndex)
        {
            return SearchGridRows[RowIndex].TableCells;
        }

        protected CheckBox chkSelect(string OfferKey)
        {
            return SearchGrid.TableCell(Find.ByText(OfferKey)).ContainingTableRow.CheckBox(Find.Any);
        }

        protected CheckBox chkSelect(int RowIndex)
        {
            return SearchGridRows[RowIndex].CheckBox(Find.Any);
        }
    }
}