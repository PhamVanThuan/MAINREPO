using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class MarkCATSPaymentBatchAsFailedCommand : ServiceCommand, ICATSServiceCommand
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public MarkCATSPaymentBatchAsFailedCommand(int cATSPaymentBatchKey)
        {
            this.CATSPaymentBatchKey = cATSPaymentBatchKey;
        }
    }
}
