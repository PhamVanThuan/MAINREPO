using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Core.UI.Halo.Editors.Address.RemoveAddressEditor
{
    public class RemoveAddressEditor : IEditor
    {
        public void Initialise(BusinessModel.BusinessContext businessContext)
        {
        }

        public IEnumerable<IUIValidationResult> SubmitEditor(IEnumerable<IEditorPageModel> submittedPageModels)
        {
            throw new NotImplementedException();
        }
    }
}