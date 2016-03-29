using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class ComplexTestCommand : ServiceCommand, ICapitecServiceCommand
    {
        [Required]
        public Applicant Applicant { get; protected set; }

        public ComplexTestCommand(Applicant applicant)
        {
            this.Applicant = applicant;
        }
    }
}
