using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class LinkPostalAddressToClientCommand : ServiceCommand, IAddressDomainCommand, IRequiresClient
    {
        [Required]
        public PostalAddressModel PostalAddressModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey")]
        public int ClientKey { get; protected set; }

        [Required]
        public Guid ClientAddressGuid { get; protected set; }

        public LinkPostalAddressToClientCommand(PostalAddressModel postalAddressModel, int clientKey, Guid clientAddressGuid)
        {
            this.ClientKey = clientKey;
            this.PostalAddressModel = postalAddressModel;
            this.ClientAddressGuid = clientAddressGuid;
        }
    }
}