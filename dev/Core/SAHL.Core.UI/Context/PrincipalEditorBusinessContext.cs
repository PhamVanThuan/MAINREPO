using System.Security.Principal;

namespace SAHL.Core.UI.Context
{
    public class PrincipalEditorBusinessContext
    {
        public PrincipalEditorBusinessContext(IPrincipal user, EditorBusinessContext editrorBusinessContext)
        {
            this.User = user;
            this.BusinessContext = editrorBusinessContext;
        }

        public IPrincipal User { get; protected set; }

        public EditorBusinessContext BusinessContext { get; protected set; }
    }
}