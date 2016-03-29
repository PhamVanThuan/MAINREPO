using SAHL.Core.Services;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class AddFreeTextAddressCommand : ServiceCommand, IAddressDomainInternalCommand
    {
        public AddFreeTextAddressCommand(FreeTextAddressModel freeTextAddress, Guid addressId)
        {
            this.FreeTextAddress = freeTextAddress;
            this.AddressId = addressId;
        }

        [Required]
        public Guid AddressId { get; protected set; }

        [Required]
        public FreeTextAddressModel FreeTextAddress { get; protected set; }
    }
}