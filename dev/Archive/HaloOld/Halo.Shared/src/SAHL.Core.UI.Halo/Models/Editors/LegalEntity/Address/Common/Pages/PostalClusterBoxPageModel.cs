using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class PostalClusterBoxPageModel : AbstractAddressPageModelBase, IEditorPageModel
    {
        public string ClusterBoxNumber { get; set; }

        public string PostOffice { get; set; }

        public void Initialise(BusinessContext businessContext)
        {
        }
    }
}