using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class PostalBoxPageModel : AbstractAddressPageModelBase, IEditorPageModel
    {
        public PostalBoxPageModel()
        {
        }

        public string BoxNumber { get; set; }

        public string PostOffice { get; set; }

        public virtual void Initialise(BusinessContext businessContext)
        {
        }
    }
}