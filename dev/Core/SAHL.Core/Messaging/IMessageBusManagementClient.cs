using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging
{
    public interface IMessageBusManagementClient : IDisposable
    {
        List<string> GetQueuesWithConsumers();
    }
}
