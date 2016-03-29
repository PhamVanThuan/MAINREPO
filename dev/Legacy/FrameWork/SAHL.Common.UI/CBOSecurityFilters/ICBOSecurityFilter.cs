using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Common.UI.CBOSecurityFilters
{
    public interface ICBOSecurityFilter
    {
        bool ApplyToChildren { get;}
        void FilterContextNodes(List<CBONode> ContextNodes);
    }
}
