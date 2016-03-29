using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ConsultantDeclineControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button Submit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_gvProposals_DXMainTable")]
        protected Table ProposalToDeclineTable { get; set; }

        protected TableRowCollection ProposalToDeclineTableRows
        {
            get
            {
                return ProposalToDeclineTable.TableRows.Filter(Find.ById(new System.Text.RegularExpressions.Regex(@"^ctl00_Main_gvProposals_DXDataRow[0-9]*$")));
            }
        }

        protected TableRow ProposalToDeclineTableRow(string type, string status)
        {
            TableRowCollection rows = ProposalToDeclineTableRows;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(type)).Exists && row.TableCell(Find.ByText(status)).Exists)
                    return row;
            }
            throw new WatiN.Core.Exceptions.WatiNException("");
        }
    }
}