using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddApplicationMailingAddressCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication
    {
        [Required]
        public ApplicationMailingAddressModel model { get; protected set; }

        public int ApplicationNumber { get { return model.ApplicationNumber; } }

        public AddApplicationMailingAddressCommand(ApplicationMailingAddressModel model)
        {
            this.model = model;
        }
    }
}