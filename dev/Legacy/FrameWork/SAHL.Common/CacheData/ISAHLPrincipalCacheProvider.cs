using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.CacheData
{
    public interface ISAHLPrincipalCacheProvider
    {
        ISAHLPrincipalCache GetPrincipalCache();

        ISAHLPrincipalCache GetPrincipalCache(IDomainMessageCollection domainMessages);

        ISAHLPrincipalCache GetPrincipalCache(SAHLPrincipal principal);

        ISAHLPrincipalCache GetPrincipalCache(SAHLPrincipal principal, IDomainMessageCollection domainMessages);

        ISAHLPrincipalProvider GetPrincipalProvider();
    }
}