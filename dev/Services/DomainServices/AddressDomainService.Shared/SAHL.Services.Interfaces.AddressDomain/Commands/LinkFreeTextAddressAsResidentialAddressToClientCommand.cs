using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class LinkFreeTextAddressAsResidentialAddressToClientCommand : ServiceCommand, IAddressDomainCommand, IRequiresClient
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey")]
        public int ClientKey { get; protected set; }

        [Required]
        public FreeTextAddressModel FreeTextAddressModel { get; protected set; }

        [Required]
        public Guid ClientAddressGuid { get; protected set; }

        public LinkFreeTextAddressAsResidentialAddressToClientCommand(FreeTextAddressModel freeTextAddressModel, int clientKey, Guid clientAddressGuid)
        {
            this.FreeTextAddressModel = freeTextAddressModel;
            this.ClientKey = clientKey;
            this.ClientAddressGuid = clientAddressGuid;
        }
    }
}