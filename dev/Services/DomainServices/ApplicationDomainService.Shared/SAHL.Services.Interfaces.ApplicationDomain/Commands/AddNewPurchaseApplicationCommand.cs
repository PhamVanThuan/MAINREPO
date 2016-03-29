using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddNewPurchaseApplicationCommand : ServiceCommand, IApplicationDomainCommand
    {
        [Required]
        public NewPurchaseApplicationModel NewPurchaseApplication { get; protected set; }

        [Required]
        public Guid ApplicationId { get; protected set; }

        public AddNewPurchaseApplicationCommand(NewPurchaseApplicationModel newPurchaseApplication, Guid applicationId)
        {
            this.NewPurchaseApplication = newPurchaseApplication;
            this.ApplicationId = applicationId;
        }
    }
}