using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class DoesApplicantExistQueryResult
    {
        public bool ExistingApplicant { get; set; }

        public Guid ApplicantID { get; set; }
    }
}
