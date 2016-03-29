using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class ChangeRibbonMenuSelectionCommand : ServiceCommand, IHaloServiceCommand
    {
        public ChangeRibbonMenuSelectionCommand(string userName, string url)
        {
            this.UserName = userName;
            this.Url = url;
        }

        public string UserName { get; protected set; }

        public string Url { get; protected set; }
    }
}