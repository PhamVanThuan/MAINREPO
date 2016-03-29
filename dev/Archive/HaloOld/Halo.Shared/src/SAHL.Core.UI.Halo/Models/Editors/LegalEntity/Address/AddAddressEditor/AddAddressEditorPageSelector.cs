using SAHL.Core.UI.Halo.Editors.Address.AddAddressEditor.Pages;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.AddAddressEditor
{
    public class AddAddressEditorPageSelector : IEditorPageModelSelector<AddAddressEditor>
    {
        public void Initialise(AddAddressEditor editor)
        {
        }

        public UIEditorPageTypeModel GetFirstPage()
        {
            return new UIEditorPageTypeModel(typeof(AddressTypeAndFormatPageModel), true, false);
        }

        public UIEditorPageTypeModel GetNextPage(IEditorPageModel currentPageModel)
        {
            return this.GetNextPageInternal((dynamic)currentPageModel);
        }

        public UIEditorPageTypeModel GetPreviousPage(IEditorPageModel currentPageModel)
        {
            return new UIEditorPageTypeModel(typeof(AddressTypeAndFormatPageModel), true, false);
        }

        private UIEditorPageTypeModel GetNextPageInternal(AddressTypeAndFormatPageModel currentPageModel)
        {
            if (currentPageModel.SelectedAddressTypeKey == 1 /* Residential */)
            {
                switch (currentPageModel.SelectedAddressFormatKey)
                {
                    case 1: // street
                        return new UIEditorPageTypeModel(typeof(ResidentialStreetPageModel), false, true);

                    case 2: // free text
                        return new UIEditorPageTypeModel(typeof(ResidentialFreeTextPageModel), false, true);
                }
            }
            else
                if (currentPageModel.SelectedAddressTypeKey == 2 /* Postal */)
                {
                    switch (currentPageModel.SelectedAddressFormatKey)
                    {
                        case 1: // street
                            return new UIEditorPageTypeModel(typeof(PostalStreetPageModel), false, true);

                        case 2: // box
                            return new UIEditorPageTypeModel(typeof(PostalBoxPageModel), false, true);

                        case 3: // postnet suite
                            return new UIEditorPageTypeModel(typeof(PostalPostNetSuitePageModel), false, true);

                        case 4: // private bag
                            return new UIEditorPageTypeModel(typeof(PostalPrivateBagPageModel), false, true);

                        case 6: // cluster box
                            return new UIEditorPageTypeModel(typeof(PostalClusterBoxPageModel), false, true);
                    }
                }

            return null;
        }
    }
}