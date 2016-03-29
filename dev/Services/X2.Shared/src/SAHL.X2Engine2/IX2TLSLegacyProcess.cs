using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2
{
    public interface IX2TLSLegacyProcess : IX2TLSProcess
    {
        void ClearCache(ISystemMessageCollection messages, Object data);
    }
}
