using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class GenericMemoControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ExpiryDateUpdate")]
        protected TextField ExpiryDate { get; set; }

        [FindBy(Id = "ctl00_Main_ReminderDateUpdate")]
        protected TextField ReminderDate { get; set; }

        [FindBy(Id = "ctl00_Main_MemoUpdate")]
        protected TextField MemoText { get; set; }

        [FindBy(Id = "ctl00_Main_MemoStatusUpdate")]
        protected SelectList ddlMemoStatus { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMemoStatus")]
        protected SelectList AccountStatus { get; set; }

        [FindBy(Id = "ctl00_Main_MemoRecordsGrid")]
        protected Table MemoGrid { get; set; }

        protected TableRow gridSelectMemo(string CellValue)
        {
            TableCellCollection memoGrid = MemoGrid.TableCells;
            return memoGrid.Filter(Find.ByText(CellValue))[0].ContainingTableRow;
        }
    }
}