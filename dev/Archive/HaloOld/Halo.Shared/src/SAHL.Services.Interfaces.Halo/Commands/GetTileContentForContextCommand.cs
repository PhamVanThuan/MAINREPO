using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetTileContentForContextCommand : ServiceCommand, IServiceCommandWithReturnedData<ITileModel>, IHaloServiceCommand
    {
        public GetTileContentForContextCommand(TileBusinessContext context)
        {
            this.Context = context;
        }

        public TileBusinessContext Context { get; protected set; }

        public ITileModel Result { get; set; }
    }
}