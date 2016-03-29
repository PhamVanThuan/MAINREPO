using SAHL.Core.UI.Halo.Editors.Address.AcceptDomiciliumEditor.Pages;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Editors.Address.AcceptDomiciliumEditor
{
    public class AcceptDomiciliumEditorPageSelector : IEditorPageModelSelector<AcceptDomiciliumEditor>
    {
        public void Initialise(AcceptDomiciliumEditor editor)
        {
        }

        public UIEditorPageTypeModel GetFirstPage()
        {
            return new UIEditorPageTypeModel(typeof(AcceptDomiciliumPageModel), true, true);
        }

        public UIEditorPageTypeModel GetNextPage(IEditorPageModel currentPageModel)
        {
            return null;
        }

        public UIEditorPageTypeModel GetPreviousPage(IEditorPageModel currentPageModel)
        {
            return null;
        }
    }
}
