using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class ResidentialStreetPageModel : AbstractAddressStreetPageModelBase, IEditorPageModel
    {
        public ResidentialStreetPageModel()
            : base()
        {
        }

        public virtual void Initialise(BusinessContext businessContext)
        {
        }

    }
}