using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class UpdateActiveNaturalPersonClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        public int ClientKey { get; protected set; }

        [Required]
        public ActiveNaturalPersonClientModel ActiveNaturalPersonClient { get; protected set; }

        public UpdateActiveNaturalPersonClientCommand(int clientKey, ActiveNaturalPersonClientModel activeNaturalPersonClient)
        {
            this.ClientKey = clientKey;
            this.ActiveNaturalPersonClient = activeNaturalPersonClient;
        }
    }
}