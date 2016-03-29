namespace ObjectMaps.Pages
{
    using System.Text.RegularExpressions;
    using WatiN.Core;

    public abstract class BankingDetailsSearchLegalEntityControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_UseButton")]
        protected Button UseButton { get; set; }

        [FindBy(Id = "ctl00_Main_BankDetailsSearchGrid")]
        protected Table BankDetailsSearchGrid { get; set; }

        protected TableRowCollection BankDetailsSearchGrid_Rows
        {
            get
            {
                return BankDetailsSearchGrid.OwnTableRows.Filter(Find.ById(new Regex(@"ctl00\$Main\$BankDetailsSearchGrid_[0-9]")));
            }
        }

        protected TableCell BankDetailsSearchGrid_Cell(string searchText)
        {
            return BankDetailsSearchGrid.TableCell(Find.ByText(searchText));
        }

        protected TableRow BankDetailsSearchGrid_Row(string searchText)
        {
            return BankDetailsSearchGrid_Cell(searchText).ContainingTableRow;
        }
    }
}