using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.CacheData
{
    public class SAHLPrincipalCacheProvider : ISAHLPrincipalCacheProvider
    {
        private ISAHLPrincipalProvider principalProvider;

        public SAHLPrincipalCacheProvider(ISAHLPrincipalProvider principalProvider)
        {
            this.principalProvider = principalProvider;
        }

        public ISAHLPrincipalCache GetPrincipalCache()
        {
            return this.GetPrincipalCache(this.principalProvider.GetCurrent());
        }

        public ISAHLPrincipalCache GetPrincipalCache(IDomainMessageCollection domainMessages)
        {
            return this.GetPrincipalCache(this.principalProvider.GetCurrent(), domainMessages);
        }

        public ISAHLPrincipalCache GetPrincipalCache(SAHLPrincipal principal)
        {
            return SAHLPrincipalCache.GetPrincipalCache(principal);
        }

        public ISAHLPrincipalCache GetPrincipalCache(SAHLPrincipal principal, IDomainMessageCollection domainMessages)
        {
            ISAHLPrincipalCache cache = this.GetPrincipalCache(principal);
            cache.SetMessageCollection(domainMessages);
            return cache;
        }

        public Security.ISAHLPrincipalProvider GetPrincipalProvider()
        {
            return this.principalProvider;
        }
    }
}