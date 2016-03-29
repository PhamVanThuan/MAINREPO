using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class ProcessCATSPaymentBatchCommand : ServiceCommand, ICATSServiceCommand, IRequiresCATSPaymentBatch
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public ProcessCATSPaymentBatchCommand(int catsPaymentBatchKey)
        {
            CATSPaymentBatchKey = catsPaymentBatchKey;
        }
    }
}
