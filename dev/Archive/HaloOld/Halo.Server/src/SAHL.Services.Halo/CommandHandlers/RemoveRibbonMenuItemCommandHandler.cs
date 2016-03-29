using SAHL.Core.BusinessModel;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class RemoveRibbonMenuItemCommandHandler : IServiceCommandHandler<RemoveRibbonMenuItemCommand>
    {
        private IUserStateManager userStateManager;

        public RemoveRibbonMenuItemCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveRibbonMenuItemCommand command)
        {
            var usermenu = this.userStateManager.GetOrCreateMenuForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }));

            var businessContext = new BusinessContext(command.Context, command.BusinessKeyType, command.BusinessKey);
            usermenu.RibbonBar.RemoveRibbonMenuElement(businessContext);
            return new SystemMessageCollection();
        }
    }
}