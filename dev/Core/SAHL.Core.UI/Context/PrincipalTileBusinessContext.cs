using System.Security.Principal;

namespace SAHL.Core.UI.Context
{
    public class PrincipalTileBusinessContext
    {
        public PrincipalTileBusinessContext(IPrincipal user, TileBusinessContext tileBusinessContext)
        {
            this.User = user;
            this.BusinessContext = tileBusinessContext;
        }

        public IPrincipal User { get; protected set; }

        public TileBusinessContext BusinessContext { get; protected set; }
    }
}