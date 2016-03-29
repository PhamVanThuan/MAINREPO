using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2NodeManagementMessageProcessor
    {
        ISystemMessageCollection ProcessMessage(IX2NodeManagementMessage message);
    }
}
