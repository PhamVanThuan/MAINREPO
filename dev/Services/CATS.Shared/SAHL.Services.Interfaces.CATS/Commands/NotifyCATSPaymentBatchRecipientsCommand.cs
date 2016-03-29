using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class NotifyCATSPaymentBatchRecipientsCommand : ServiceCommand, ICATSServiceCommand, IRequiresCATSPaymentBatch
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public NotifyCATSPaymentBatchRecipientsCommand(int catsPaymentBatchKey)
        {
            CATSPaymentBatchKey = catsPaymentBatchKey;
        }
    }
}
