using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class DrillDownAndGetUsersTilesForContextCommandHandler : IServiceCommandHandler<DrillDownAndGetUsersTilesForContextCommand>
    {
        private IUserStateManager userStateManager;

        public DrillDownAndGetUsersTilesForContextCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(DrillDownAndGetUsersTilesForContextCommand command)
        {
            command.Result = this.userStateManager.DrillDownAndGetUsersTilesForContext(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.Context);
            return new SystemMessageCollection();
        }
    }
}