using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class PostalPrivateBagPageModel : AbstractAddressPageModelBase, IEditorPageModel
    {
        public string PrivateBag { get; set; }

        public string PostOffice { get; set; }

        public void Initialise(BusinessContext businessContext)
        {
        }
    }
}