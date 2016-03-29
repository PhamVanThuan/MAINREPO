using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Shared.Messages
{
    public interface IBatchMessage : IMessage
    {
        int FailureCount { get; set; }

        int BatchID { get; set; }
    }
}
