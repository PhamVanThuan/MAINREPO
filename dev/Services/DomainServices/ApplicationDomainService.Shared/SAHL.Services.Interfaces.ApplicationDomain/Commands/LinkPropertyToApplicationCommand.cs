using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class LinkPropertyToApplicationCommand : ServiceCommand, IApplicationDomainCommand, IRequiresProperty, IRequiresOpenApplication
    {
        [Required]
        public LinkPropertyToApplicationCommandModel ApplicationPropertyModel { get; protected set; }

        public int PropertyKey { get { return ApplicationPropertyModel.PropertyKey; } }

        public int ApplicationNumber { get { return ApplicationPropertyModel.ApplicationNumber; } }

        public LinkPropertyToApplicationCommand(LinkPropertyToApplicationCommandModel ApplicationPropertyModel)
        {
            this.ApplicationPropertyModel = ApplicationPropertyModel;
        }
    }
}
