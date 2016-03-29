using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Halo.Common;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public class ResidentialFreeTextPageModel : IEditorPageModel
    {
        public void Initialise(BusinessContext businessContext)
        {
        }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string Line5 { get; set; }

        public IEnumerable<NamedKey> Countries { get; protected set; }

        public int SelectedCountryKey { get; set; }
    }
}