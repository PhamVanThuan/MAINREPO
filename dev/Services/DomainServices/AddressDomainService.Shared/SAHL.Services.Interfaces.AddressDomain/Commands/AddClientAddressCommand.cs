using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class AddClientAddressCommand : ServiceCommand, IAddressDomainInternalCommand
    {
        [Required]
        public ClientAddressModel ClientAddressModel { get; protected set; }

        [Required]
        public Guid ClientAddressGuid { get; protected set; }

        public AddClientAddressCommand(ClientAddressModel clientAddressModel, Guid clientAddressGuid)
        {
            this.ClientAddressModel = clientAddressModel;
            this.ClientAddressGuid = clientAddressGuid;
        }
    }
}