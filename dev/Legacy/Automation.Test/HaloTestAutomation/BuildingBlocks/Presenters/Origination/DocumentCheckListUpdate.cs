using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    /// <summary>
    /// The Document Checklist Update screen. Class contains methods pertaining to the Document Checklist.
    /// </summary>
    public class DocumentCheckListUpdate : DocumentChecklistUpdateControls
    {
        private readonly IWatiNService watinService;

        public DocumentCheckListUpdate()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Finds all of the checkboxes for the document checklist and updates them to be ticked.
        /// </summary>
        public void UpdateDocumentChecklist()
        {
            watinService.GenericCheckAllCheckBoxes(base.Document.DomContainer);
            base.btnSubmit.Click();
        }
    }
}