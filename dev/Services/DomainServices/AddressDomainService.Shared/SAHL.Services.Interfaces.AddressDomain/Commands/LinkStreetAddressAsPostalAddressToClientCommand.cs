using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class LinkStreetAddressAsPostalAddressToClientCommand : ServiceCommand, IAddressDomainCommand, IRequiresClient
    {
        [Required]
        public StreetAddressModel StreetAddressModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey")]
        public int ClientKey { get; protected set; }

        [Required]
        public Guid ClientAddressGuid { get; protected set; }

        public LinkStreetAddressAsPostalAddressToClientCommand(StreetAddressModel streetAddressModel, int clientKey, Guid clientAddressGuid)
        {
            this.StreetAddressModel = streetAddressModel;
            this.ClientKey = clientKey;
            this.ClientAddressGuid = clientAddressGuid;
        }
    }
}