using System;
using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor
{
    public class ChangeDomiciliumAddressEditor : IEditor
    {
        public string Title
        {
            get { return "Change Domicilium Address"; }
        }

        public void Initialise(BusinessContext businessContext)
        {
        }

        public IEnumerable<IUIValidationResult> SubmitEditor(IEnumerable<IEditorPageModel> submittedPageModels)
        {
            throw new NotImplementedException();
        }
    }
}