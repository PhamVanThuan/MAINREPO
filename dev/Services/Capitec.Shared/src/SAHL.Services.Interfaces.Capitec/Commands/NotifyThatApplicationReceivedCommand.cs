using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class NotifyThatApplicationReceivedCommand : ServiceCommand, ICapitecServiceCommand
    {
        public NotifyThatApplicationReceivedCommand(IEnumerable<Applicant> applicants, int applicationNumber)
        {
            this.Applicants = applicants;
            this.ApplicationNumber = applicationNumber;
        }        
        public IEnumerable<Applicant> Applicants { get; protected set; }   
        public  int ApplicationNumber { get; protected set; }
    }
}