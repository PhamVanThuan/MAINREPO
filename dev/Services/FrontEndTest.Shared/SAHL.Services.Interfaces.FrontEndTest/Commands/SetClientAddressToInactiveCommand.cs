using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class SetClientAddressToInactiveCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int ClientAddressKey { get; protected set; }

        public SetClientAddressToInactiveCommand(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
        }
    }
}