using SAHL.Common.Logging;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public class DefaultMessageRetryService<TMessage> : IMessageRetryService<TMessage> where TMessage : class, IBatchMessage
    {

        public DefaultMessageRetryService()
        {

        }

        public void Start()
        {
            return;
        }

        public void Reset()
        {
            return;
        }
    }
}
