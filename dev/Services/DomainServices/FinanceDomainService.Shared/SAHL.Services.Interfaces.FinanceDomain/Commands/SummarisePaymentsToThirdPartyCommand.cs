using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class SummarisePaymentsToThirdPartyCommand : ServiceCommand, IFinanceDomainCommand, IRequiresCATSPaymentBatch
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public SummarisePaymentsToThirdPartyCommand(int catsPaymentBatchKey)
        {
            CATSPaymentBatchKey = catsPaymentBatchKey;
        }
    }
}