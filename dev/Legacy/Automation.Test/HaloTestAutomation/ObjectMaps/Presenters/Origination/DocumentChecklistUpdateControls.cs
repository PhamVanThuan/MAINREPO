using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DocumentChecklistUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_DocCheckListGrid")]
        protected Table DocChecklist { get; set; }

        protected TableRow DocCheckListRow(string documentName)
        {
            return DocChecklist.TableCell(Find.ByText(documentName)).ContainingTableRow;
        }
    }
}