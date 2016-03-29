using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.UpdateAddressEditor
{
    public class UpdateAddressEditorPageSelector : IEditorPageModelSelector<UpdateAddressEditor>
    {
        private UpdateAddressEditor editor;

        public void Initialise(UpdateAddressEditor editor)
        {
            this.editor = editor;
        }

        public UIEditorPageTypeModel GetFirstPage()
        {
            switch (this.editor.AddressTypeKey)
            {
                case 1: // residential
                    switch (this.editor.AddressFormatKey)
                    {
                        case 1: // street
                            return new UIEditorPageTypeModel(typeof(ResidentialStreetPageModel), true, true);

                        case 5: // box
                            return new UIEditorPageTypeModel(typeof(ResidentialFreeTextPageModel), true, true);
                    }
                    break;

                case 2: // postal
                    switch (this.editor.AddressFormatKey)
                    {
                        case 1: // street
                            return new UIEditorPageTypeModel(typeof(PostalStreetPageModel), true, true);

                        case 2: // box
                            return new UIEditorPageTypeModel(typeof(PostalBoxPageModel), true, true);

                        case 3: // postnet suite
                            return new UIEditorPageTypeModel(typeof(PostalPostNetSuitePageModel), true, true);

                        case 4: // private bag
                            return new UIEditorPageTypeModel(typeof(PostalPrivateBagPageModel), true, true);

                        case 6: // cluster box
                            return new UIEditorPageTypeModel(typeof(PostalClusterBoxPageModel), true, true);
                    }
                    break;
            }
            return null;
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