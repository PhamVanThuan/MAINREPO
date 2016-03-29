using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Areas;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetUsersTilesForContextCommand : ServiceCommand, IServiceCommandWithReturnedData<TileElementArea>, IHaloServiceCommand
    {
        public GetUsersTilesForContextCommand(TileBusinessContext context, string userName)
        {
            this.UserName = userName;
            this.Context = context;
        }

        public string UserName { get; protected set; }

        public TileBusinessContext Context { get; protected set; }

        public TileElementArea Result { get; set; }
    }
}