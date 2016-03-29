using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresThirdParty : IDomainCommandCheck
    {
        Guid ThirdPartyId { get; }
    }
}