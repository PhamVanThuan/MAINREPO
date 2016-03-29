using System.Security.Principal;

namespace SAHL.Core.BusinessModel
{
    public class PrincipalBusinessContext
    {
        public PrincipalBusinessContext(IPrincipal user, BusinessContext businessContext)
        {
            this.User = user;
            this.BusinessContext = businessContext;
        }

        public IPrincipal User { get; protected set; }

        public BusinessContext BusinessContext { get; protected set; }
    }
}