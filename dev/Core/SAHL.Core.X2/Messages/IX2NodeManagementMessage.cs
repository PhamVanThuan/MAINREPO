using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.X2.Messages
{
    public interface IX2NodeManagementMessage : IX2Message
    {
        X2ManagementType ManagementType { get; }

        Object Data { get; }
    }
}
