using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddApplicationDebitOrderCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication
    {
        [Required]
        public ApplicationDebitOrderModel ApplicationDebitOrderModel { get; protected set; }

        public int ApplicationNumber { get { return ApplicationDebitOrderModel.ApplicationNumber; } }

        public AddApplicationDebitOrderCommand(ApplicationDebitOrderModel applicationDebitOrderModel)
        {
            this.ApplicationDebitOrderModel = applicationDebitOrderModel;
        }
    }
}