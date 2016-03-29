using System;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebtCounsellingProposalSummaryWorkflowControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_btnView")]
        protected Button btnView { get; set; }

        [FindBy(Id = "ctl00_Main_btnCopyToDraft")]
        protected Button btnCopyToDraft { get; set; }

        [FindBy(Id = "ctl00_Main_btnSetActive")]
        protected Button btnSetActive { get; set; }

        [FindBy(Id = "ctl00_Main_btnDelete")]
        protected Button btnDelete { get; set; }

        [FindBy(Id = "ctl00_Main_gridProposalSummary_DXMainTable")]
        protected Table gridProposalSummaryDXMainTable { get; set; }

        [FindBy(Id = "ctl00_Main_btnCreateCounterProposal")]
        protected Button CreateCounterProposal { get; set; }

        protected TableRowCollection gridProposalSummaryDXMainTableRows
        {
            get
            {
                return gridProposalSummaryDXMainTable.TableRows.Filter(Find.
                    ById(new System.Text.RegularExpressions.Regex(@"^ctl00_Main_gridProposalSummary_DXDataRow[0-9]*$")));
            }
        }

        [FindBy(Class = "dxgvSelectedRow_SoftOrange action")]
        protected TableRow SelectedRow { get; set; }

        protected TableRow gridProposalSummaryDXMainTableRow(string type, string status, DateTime dateCaptured)
        {
            TableRowCollection rows = gridProposalSummaryDXMainTableRows;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(type)).Exists && row.TableCell(Find.ByText(status)).Exists && row.TableCell(Find.ByText(dateCaptured.ToString(@"dd/MM/yyyy HH:mm:ss"))).Exists)
                    return row;
            }
            throw new WatiN.Core.Exceptions.WatiNException("");
        }

        protected TableRow gridProposalSummaryDXMainTableRow(string type, string status)
        {
            TableRowCollection rows = gridProposalSummaryDXMainTableRows;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(type)).Exists && row.TableCell(Find.ByText(status)).Exists)
                    return row;
            }
            return null;
        }

        protected bool gridProposalSummaryDXMainTableRowExists(string type, string status)
        {
            TableRowCollection rows = gridProposalSummaryDXMainTableRows;

            bool result = false;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(type)).Exists && row.TableCell(Find.ByText(status)).Exists)
                    result = true;
            }
            return result;
        }

        protected TableCellCollection gridProposalSummaryDXMainTableCells(int row)
        {
            return gridProposalSummaryDXMainTableRows[row].TableCells;
        }

        protected TableCell gridProposalSummaryDXMainTableCell(int row, int col)
        {
            return gridProposalSummaryDXMainTableCells(row)[col];
        }

        [FindBy(Id = "ctl00_Main_btnReasons")]
        protected Button btnReasons { get; set; }
    }
}