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
    public class DetermineApplicationHouseholdIncomeCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication
    {
        [Required]
        public int ApplicationNumber { get; protected set; }

        public DetermineApplicationHouseholdIncomeCommand(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }
    }
}
