using SAHL.Core.Services;
using SAHL.Core.UI.Elements.Areas;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetUserMenuCommand : ServiceCommand, IServiceCommandWithReturnedData<MenuElementArea>, IHaloServiceCommand
    {
        public GetUserMenuCommand(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; protected set; }

        public MenuElementArea Result { get; set; }
    }
}