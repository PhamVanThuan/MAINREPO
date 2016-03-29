using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public interface IMessageRetryService<TMessage> where TMessage : class,IMessage
    {
        void Start();

        void Reset();
    }
}
