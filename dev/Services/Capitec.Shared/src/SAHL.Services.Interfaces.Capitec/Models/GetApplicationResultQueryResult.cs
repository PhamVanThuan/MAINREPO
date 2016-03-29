using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetApplicationResultQueryResult
    {
        public bool Submitted { get; set; }
        public int ApplicationNumber { get; set; }
        public ISystemMessageCollection ApplicationCalculationMessages { get; set; }

        public string FirstApplicantName { get; set; }
        public bool FirstApplicantITCPassed { get; set; }
        public ISystemMessageCollection FirstApplicantITCMessages { get; set; }

        public string SecondApplicantName { get; set; }
        public bool SecondApplicantITCPassed { get; set; }
        public ISystemMessageCollection SecondApplicantITCMessages { get; set; }
    }
}
