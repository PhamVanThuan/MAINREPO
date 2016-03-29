using SAHL.Core.Services;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class AddPostalAddressCommand : ServiceCommand, IAddressDomainInternalCommand
    {
        [Required]
        public PostalAddressModel PostalAddressModel { get; protected set; }

        [Required]
        public Guid AddressId { get; protected set; }

        public AddPostalAddressCommand(PostalAddressModel PostalAddressModel, Guid AddressId)
        {
            this.PostalAddressModel = PostalAddressModel;
            this.AddressId = AddressId;
        }
    }
}