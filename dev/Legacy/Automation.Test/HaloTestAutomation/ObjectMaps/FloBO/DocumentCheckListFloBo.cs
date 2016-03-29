using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class DocumentCheckListNodeControls : BaseNavigation
    {
        [FindBy(Text = "Document Checklist")]
        protected Link DocumentChecklist { get; set; }

        [FindBy(Text = "View Document Checklist")]
        protected Link ViewDocumentChecklist { get; set; }

        [FindBy(Text = "Update Document Checklist")]
        protected Link UpdateDocumentChecklist { get; set; }
    }
}