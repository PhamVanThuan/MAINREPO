using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Common.Service.Interfaces
{
    public interface ICBOSecurityFilter
    {
        bool ApplyToChildren { get;}
        void FilterContextNodes(IEventList<CBONode> ContextNodes);
    }
}
