using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System.ComponentModel.DataAnnotations;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System.Collections.Generic;
using System;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class AddApplicantsCommand : ServiceCommand, ICapitecServiceInternalCommand
    {
        public AddApplicantsCommand(Dictionary<Guid, Applicant> applicants)
        {
            this.Applicants = applicants;           
        }

        [Required]
        public Dictionary<Guid, Applicant> Applicants { get; protected set; }     
    }
}
