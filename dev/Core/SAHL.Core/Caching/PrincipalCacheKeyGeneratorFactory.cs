using System.Security.Principal;

namespace SAHL.Core.Caching
{
    public class PrincipalCacheKeyGeneratorFactory : AbstractCacheKeyGeneratorFactory<IPrincipal>
    {
        public PrincipalCacheKeyGeneratorFactory(IHashGenerator hashGenerator)
            : base(hashGenerator)
        {
        }

        protected override string GetContextString(IPrincipal context)
        {
            return context.Identity.Name;
        }
    }
}