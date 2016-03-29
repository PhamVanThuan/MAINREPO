using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class UpdateInactiveNaturalPersonClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        public int ClientKey { get; protected set; }

        [Required]
        public NaturalPersonClientModel NaturalPersonClient { get; protected set; }

        public UpdateInactiveNaturalPersonClientCommand(int clientKey, NaturalPersonClientModel naturalPersonClient)
        {
            this.NaturalPersonClient = naturalPersonClient;
            this.ClientKey = clientKey;
        }
    }
}