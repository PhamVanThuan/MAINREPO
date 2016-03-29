using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetTileContentForContextCommandHandler : IServiceCommandHandler<GetTileContentForContextCommand>
    {
        private ITileManager tileManager;

        public GetTileContentForContextCommandHandler(ITileManager tileManager)
        {
            this.tileManager = tileManager;
        }

        public ISystemMessageCollection HandleCommand(GetTileContentForContextCommand command)
        {
            command.Result = this.tileManager.GetTileContent(command.Context);
            return new SystemMessageCollection();
        }
    }
}