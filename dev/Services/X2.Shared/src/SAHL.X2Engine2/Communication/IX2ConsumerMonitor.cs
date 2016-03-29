using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2ConsumerMonitor : IDisposable
    {
        void Initialise();

        bool IsExchangeActive(string exchange, X2Workflow workflow);
    }
}
