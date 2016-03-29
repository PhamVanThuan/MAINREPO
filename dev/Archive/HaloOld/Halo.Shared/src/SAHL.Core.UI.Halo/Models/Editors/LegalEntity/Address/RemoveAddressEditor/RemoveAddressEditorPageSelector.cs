using SAHL.Core.UI.Halo.Editors.Address.RemoveAddressEditor.Pages;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Editors.Address.RemoveAddressEditor
{
    public class RemoveAddressEditorPageSelector : IEditorPageModelSelector<RemoveAddressEditor>
    {
        private RemoveAddressEditor editor;

        public void Initialise(RemoveAddressEditor editor)
        {
            this.editor = editor;
        }

        public UIEditorPageTypeModel GetFirstPage()
        {
            return new UIEditorPageTypeModel(typeof(RemoveAddressConfirmPageModel), true, true);
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