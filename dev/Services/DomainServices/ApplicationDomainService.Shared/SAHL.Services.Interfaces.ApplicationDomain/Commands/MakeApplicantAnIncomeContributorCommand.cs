using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class MakeApplicantAnIncomeContributorCommand : ServiceCommand, IApplicationDomainCommand, IRequiresActiveClientRole
    {
        [Required]
        public int ApplicationRoleKey { get; protected set; }

        public MakeApplicantAnIncomeContributorCommand(int applicationRoleKey)
        {
            this.ApplicationRoleKey = applicationRoleKey;
        }
    }
}
