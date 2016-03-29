using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddLeadApplicantToApplicationCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication, IRequiresClient
    {
        [Required]
        public Guid ApplicationRoleId { get; protected set; }

        [Required]
        public int ClientKey { get; protected set; }

        [Required]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public LeadApplicantOfferRoleTypeEnum ApplicationRoleType { get; protected set;}

        public AddLeadApplicantToApplicationCommand(Guid applicationRoleId, int clientKey, int applicationNumber, LeadApplicantOfferRoleTypeEnum applicationRoleType)
        {
            this.ApplicationRoleId = applicationRoleId;
            this.ClientKey = clientKey;
            this.ApplicationNumber = applicationNumber;
            this.ApplicationRoleType = applicationRoleType;
        }
    }
}