using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddNaturalPersonClientCommand : ServiceCommand, IClientDomainCommand
    {
        [Required]
        public NaturalPersonClientModel NaturalPersonClient { get; protected set; }

        public AddNaturalPersonClientCommand(NaturalPersonClientModel naturalPersonClient)
        {
            this.NaturalPersonClient = naturalPersonClient;
        }
    }
}