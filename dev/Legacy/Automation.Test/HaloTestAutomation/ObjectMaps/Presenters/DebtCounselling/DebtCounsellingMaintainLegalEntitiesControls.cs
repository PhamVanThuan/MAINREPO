using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebtCounsellingMaintainLegalEntitiesControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button Remove { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button Add { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_grdApplicants")]
        public Table ctl00MaingrdApplicantsMainTable { get; set; }

        public TableRowCollection ctl00MaingrdApplicantsMainTableRows
        {
            get
            {
                return ctl00MaingrdApplicantsMainTable.TableRows;
            }
        }

        public TableRow ctl00MaingrdApplicantsMainTableRow(string idnumber)
        {
            TableRowCollection rows = ctl00MaingrdApplicantsMainTableRows;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(idnumber)).Exists)
                    return row;
            }
            throw new WatiN.Core.Exceptions.WatiNException("");
        }

        [FindBy(Id = "ctl00_Main_lblLinkedDebtCounsellingAccountsWarningMessage")]
        protected Span GroupedAccountWarningLabel { get; set; }
    }
}