using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class ChangeUserRoleCommandHandler : IServiceCommandHandler<ChangeUserRoleCommand>
    {
        private IUserStateManager userStateManager;

        public ChangeUserRoleCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(ChangeUserRoleCommand command)
        {
            var userDetails = this.userStateManager.GetUserDetailsForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }));
            userDetails.ChangeActiveRole(command.OrganisationArea, command.RoleName);
            return new SystemMessageCollection();
        }
    }
}