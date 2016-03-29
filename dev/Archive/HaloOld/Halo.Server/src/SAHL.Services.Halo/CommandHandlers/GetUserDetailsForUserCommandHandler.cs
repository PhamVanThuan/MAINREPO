using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetUserDetailsForUserCommandHandler : IServiceCommandHandler<GetUserDetailsForUserCommand>
    {
        private IUserStateManager userStateManager;

        public GetUserDetailsForUserCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(GetUserDetailsForUserCommand command)
        {
            command.Result = this.userStateManager.GetUserDetailsForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }));
            return new SystemMessageCollection();
        }
    }
}