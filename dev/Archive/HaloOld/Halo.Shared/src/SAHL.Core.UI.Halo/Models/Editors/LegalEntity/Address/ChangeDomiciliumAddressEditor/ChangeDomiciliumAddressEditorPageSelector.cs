using SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor.Pages;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.ChangeDomiciliumAddressEditor
{
    public class ChangeDomiciliumAddressEditorPageSelector : IEditorPageModelSelector<SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor.ChangeDomiciliumAddressEditor>
    {
        private SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor.ChangeDomiciliumAddressEditor editor;

        public void Initialise(SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor.ChangeDomiciliumAddressEditor editor)
        {
            this.editor = editor;
        }

        public UIEditorPageTypeModel GetFirstPage()
        {
            return new UIEditorPageTypeModel(typeof(ChangeDomiciliumAddressEditorPage1Model), true, true);
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