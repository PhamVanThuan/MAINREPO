using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class BankingDetailsDisplayControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_BankDetailsGrid")]
        protected Table tblBankDetailsGrid { get; set; }

        protected TableRowCollection tblRowsBankDetailsGrid
        {
            get
            {
                return tblBankDetailsGrid.TableRows;
            }
        }

        protected TableCell tblCellBankDetailsGrid(int rowNumber, int columnHeader)
        {
            return tblBankDetailsGrid.TableRows[rowNumber].TableCells[columnHeader];
        }
    }
}