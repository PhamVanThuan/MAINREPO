using SAHL.Core.UI.Elements.Areas;
using SAHL.Core.UI.UserState.Models;
using SAHL.Services.Interfaces.Halo;
using StructureMap;

namespace SAHL.Website.Halo.App_Start.DependencyResolution
{
    public static class StaticHelpers
    {
        public static MenuElementArea GetUserMenu(System.Security.Principal.IPrincipal user)
        {
            var haloService = ObjectFactory.GetInstance<IHaloService>();
            var command = new SAHL.Services.Interfaces.Halo.Commands.GetUserMenuCommand(user.Identity.Name);
            var result = haloService.PerformCommand(command);
            var menuArea = command.Result;
            return menuArea;
        }

        public static IUserDetails GetUserDetailsForUser(System.Security.Principal.IPrincipal user)
        {
            var haloService = ObjectFactory.GetInstance<IHaloService>();
            var command = new SAHL.Services.Interfaces.Halo.Commands.GetUserDetailsForUserCommand(user.Identity.Name);
            var result = haloService.PerformCommand(command);
            var userDetails = command.Result;
            return userDetails;
        }
    }
}