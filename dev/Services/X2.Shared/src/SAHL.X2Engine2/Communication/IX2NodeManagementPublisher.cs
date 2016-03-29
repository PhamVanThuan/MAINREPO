using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2NodeManagementPublisher
    {
        void Publish<TMessage>(TMessage message) where TMessage : class, IX2NodeManagementMessage;
    }
}
