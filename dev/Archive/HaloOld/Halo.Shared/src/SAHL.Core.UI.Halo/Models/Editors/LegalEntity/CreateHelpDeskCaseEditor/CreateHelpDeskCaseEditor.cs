using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor
{
    public class CreateHelpDeskCaseEditor : IEditor
    {
        public string Title
        {
            get { return "Create Help Desk Case"; }
        }

        public void Initialise(BusinessContext businessContext)
        {
        }

        public System.Collections.Generic.IEnumerable<IUIValidationResult> SubmitEditor(System.Collections.Generic.IEnumerable<IEditorPageModel> submittedPageModels)
        {
            throw new System.NotImplementedException();
        }
    }
}