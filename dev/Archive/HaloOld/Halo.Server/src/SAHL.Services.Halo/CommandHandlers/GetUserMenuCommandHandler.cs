using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetUserMenuCommandHandler : IServiceCommandHandler<GetUserMenuCommand>
    {
        private IUserStateManager userStateManager;

        public GetUserMenuCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(GetUserMenuCommand command)
        {
            command.Result = this.userStateManager.GetOrCreateMenuForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }));
            return new SystemMessageCollection();
        }
    }
}