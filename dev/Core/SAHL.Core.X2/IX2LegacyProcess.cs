using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.X2
{
    public interface IX2LegacyProcess : IX2Process
    {
        void ClearCache(ISystemMessageCollection messages, Object data);
    }
}
