using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class X2WorklistControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_gridInstance_DXMainTable")]
        protected Table SearchGrid { get; set; }

        protected TableRowCollection SearchGridRows
        {
            get
            {
                return SearchGrid.TableRows.Filter(Find.ById(new Regex("^ctl00_Main_gridInstance_DXDataRow[0-9]*$")));
            }
        }

        protected TableCellCollection SearchGridCells
        {
            get
            {
                return SearchGrid.TableCells;
            }
        }

        protected TableCellCollection SearchGridRowCells(int RowIndex)
        {
            return SearchGridRows[RowIndex].TableCells;
        }

        protected TableRow gridSelectOffer(int offerkey)
        {
            TableCellCollection applicationCaptureWorkflow = base.Document.Table("ctl00_Main_gridInstance_DXMainTable").TableCells;
            return applicationCaptureWorkflow.Filter(Find.ByText(offerkey.ToString()))[0].ContainingTableRow;
        }

        protected TableRow gridSelectFirstOffer()
        {
            return base.Document.TableRow("ctl00_Main_gridInstance_DXDataRow0");
        }

        public bool gridOfferExists(int offerkey)
        {
            return base.Document.Table("ctl00_Main_gridInstance_DXMainTable").TableCells.Exists(Find.ByText(offerkey.ToString()));
        }

        [FindBy(Id = "ctl00_Main_btnSelect")]
        protected Button btnSelect { get; set; }

        [FindBy(Class = "dxgvPagerBottomPanel_SoftOrange")]
        public Div dxgvPagerBottomPanel { get; set; }

        public TableCellCollection dxpPageNumber
        {
            get
            {
                return base.Document.TableCells.Filter(Find.ByClass(new Regex("^dxpPageNumber_SoftOrange[\x20-\x7E]*$")));
            }
        }

        public TableCell dxpPageNumber_Current
        {
            get
            {
                return base.Document.TableCell(Find.ByClass(new Regex("dxpPageNumber_SoftOrange dxpCurrentPageNumber_SoftOrange")));
            }
        }

        [FindBy(Alt = "Prev")]
        protected Image Previous { get; set; }

        [FindBy(Alt = "Next")]
        protected Image Next { get; set; }

        [FindBy(Class = "dxWeb_pNextDisabled_SoftOrange")]
        protected Image DisabledNext { get; set; }
    }
}