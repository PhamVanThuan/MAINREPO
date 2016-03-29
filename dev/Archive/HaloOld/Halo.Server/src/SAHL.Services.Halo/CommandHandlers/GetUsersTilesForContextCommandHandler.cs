using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetUsersTilesForContextCommandHandler : IServiceCommandHandler<GetUsersTilesForContextCommand>
    {
        private IUserStateManager userStateManager;

        public GetUsersTilesForContextCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(GetUsersTilesForContextCommand command)
        {
            command.Result = this.userStateManager.GetUsersTilesForContext(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.Context);
            return new SystemMessageCollection();
        }
    }
}