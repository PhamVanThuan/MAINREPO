using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresCATSPaymentBatch : IDomainCommandCheck
    {
        int CATSPaymentBatchKey { get; }
    }
}