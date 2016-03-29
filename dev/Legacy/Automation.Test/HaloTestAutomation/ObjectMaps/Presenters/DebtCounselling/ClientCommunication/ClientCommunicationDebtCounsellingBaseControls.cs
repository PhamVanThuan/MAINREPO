using WatiN.Core;

namespace ObjectMaps
{
    public abstract class ClientCommunicationDebtCounsellingBaseControls
    {
        private Frame _frame;

        public ClientCommunicationDebtCounsellingBaseControls(Browser browser)
        {
            this._frame = browser.Frames[0];
        }

        public Image ctl00_Main_btnSend
        {
            get
            {
                return this._frame.Image(Find.ById("ctl00_Main_btnSend"));
            }
        }

        public Table ctl00_Main_gridRecipients_DXMainTable
        {
            get
            {
                return this._frame.Table("ctl00_Main_gridRecipients_DXMainTable");
            }
        }

        public TableCell this[int rowIndex, string columnName]
        {
            get
            {
                int colIndex = 0;
                //Get Column Index.
                foreach (TableCell cell in this.ctl00_Main_gridRecipients_DXHeadersRow.OwnTableCells)
                {
                    if (cell.Text.Equals(columnName))
                        break;
                    colIndex++;
                }
                TableRow row = this.ctl00_Main_gridRecipients_DXMainTable.OwnTableRows[rowIndex];
                return row.TableCells[colIndex];
            }
        }

        public TableRow ctl00_Main_gridRecipients_DXHeadersRow
        {
            get
            {
                return this._frame.TableRow("ctl00_Main_gridRecipients_DXHeadersRow");
            }
        }
    }
}