using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class LinkStreetAddressToPropertyCommand : ServiceCommand, IAddressDomainCommand, IRequiresProperty
    {
        [Required]
        public StreetAddressModel StreetAddressModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid PropertyKey")]
        public int PropertyKey { get; protected set; }

        public LinkStreetAddressToPropertyCommand(StreetAddressModel streetAddressModel, int propertyKey)
        {
            this.StreetAddressModel = streetAddressModel;
            this.PropertyKey = propertyKey;
        }
    }
}