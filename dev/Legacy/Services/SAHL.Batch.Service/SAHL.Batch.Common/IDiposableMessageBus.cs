using SAHL.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace SAHL.Batch.Common
{
    public interface IDiposableMessageBus : IDisposable, IMessageBus
    {
    }
}
