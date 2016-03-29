using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddEmployerCommand : ServiceCommand, IClientDomainCommand
    {
        [Required]
        public Guid EmployerId { get; protected set; }

        [Required]
        public EmployerModel Employer { get; protected set; }

        public AddEmployerCommand(Guid employerId, EmployerModel employer)
        {
            this.Employer = employer;
            this.EmployerId = employerId;
        }
    }
}