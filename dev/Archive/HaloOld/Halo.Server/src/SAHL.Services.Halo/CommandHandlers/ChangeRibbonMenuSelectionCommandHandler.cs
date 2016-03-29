using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class ChangeRibbonMenuSelectionCommandHandler : IServiceCommandHandler<ChangeRibbonMenuSelectionCommand>
    {
        private IUserStateManager userStateManager;

        public ChangeRibbonMenuSelectionCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(ChangeRibbonMenuSelectionCommand command)
        {
            var usermenu = this.userStateManager.GetOrCreateMenuForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }));

            var toBeSelectedUserMenu = usermenu.RibbonBar.RibbonMenuItemElements.SingleOrDefault(x => CultureInfo.CurrentCulture.CompareInfo.Compare(x.Url, command.Url, CompareOptions.IgnoreCase) == 0);
            if (toBeSelectedUserMenu != null)
                usermenu.RibbonBar.Select(toBeSelectedUserMenu);

            usermenu.ContextBar.ClearItems();

            return new SystemMessageCollection();
        }
    }
}