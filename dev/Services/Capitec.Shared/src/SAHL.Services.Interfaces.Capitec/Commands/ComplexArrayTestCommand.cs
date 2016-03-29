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
    public class ComplexArrayTestCommand : ServiceCommand, ICapitecServiceCommand
    {
        [Required]
        public IEnumerable<Applicant> Applicants { get; protected set; }

        [Required]
        public Applicant[] ApplicantArray { get; protected set; }

        public ComplexArrayTestCommand(IEnumerable<Applicant> applicants, Applicant[] applicantArray)
        {
            this.Applicants = applicants;
            this.ApplicantArray = applicantArray;
        }
    }
}
