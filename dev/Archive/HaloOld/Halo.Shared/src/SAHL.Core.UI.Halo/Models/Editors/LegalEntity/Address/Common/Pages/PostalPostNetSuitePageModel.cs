using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class PostalPostNetSuitePageModel : AbstractAddressPageModelBase, IEditorPageModel
    {
        public string PostnetSuiteNumber { get; set; }

        public string PrivateBag { get; set; }

        public string PostOffice { get; set; }

        public void Initialise(BusinessContext businessContext)
        {
        }
    }
}