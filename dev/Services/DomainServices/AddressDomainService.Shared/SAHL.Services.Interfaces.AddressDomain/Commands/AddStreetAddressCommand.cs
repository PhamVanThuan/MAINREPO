using SAHL.Core.Services;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class AddStreetAddressCommand : ServiceCommand, IAddressDomainInternalCommand
    {
        [Required]
        public StreetAddressModel StreetAddressModel { get; protected set; }

        [Required]
        public Guid AddressId { get; protected set; }

        public AddStreetAddressCommand(StreetAddressModel StreetAddressModel, Guid AddressId)
        {
            this.StreetAddressModel = StreetAddressModel;
            this.AddressId = AddressId;
        }
    }
}