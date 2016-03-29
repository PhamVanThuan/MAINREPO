using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresThirdPartyInvoice : IDomainCommandCheck
    {
        int ThirdPartyInvoiceKey { get; }
    }
}