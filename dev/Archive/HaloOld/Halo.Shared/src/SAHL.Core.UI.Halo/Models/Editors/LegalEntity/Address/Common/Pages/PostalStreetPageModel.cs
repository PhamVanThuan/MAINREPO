using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class PostalStreetPageModel : AbstractAddressStreetPageModelBase, IEditorPageModel
    {
        public PostalStreetPageModel()
            : base()
        {
        }

        public virtual void Initialise(BusinessContext businessContext)
        {
        }
    }
}