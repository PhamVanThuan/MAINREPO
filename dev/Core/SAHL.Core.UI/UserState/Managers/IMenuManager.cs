using SAHL.Core.UI.Elements.Areas;
using System.Security.Principal;

namespace SAHL.Core.UI.UserState.Managers
{
    public interface IMenuManager
    {
        MenuElementArea CreateMenuForUser(IPrincipal user);
    }
}