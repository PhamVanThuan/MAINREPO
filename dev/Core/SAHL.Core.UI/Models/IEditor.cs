using SAHL.Core.BusinessModel;
using System.Collections.Generic;

namespace SAHL.Core.UI.Models
{
    public interface IEditor
    {
        void Initialise(BusinessContext businessContext);

        IEnumerable<IUIValidationResult> SubmitEditor(IEnumerable<IEditorPageModel> submittedPageModels);
    }
}