using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class DoesPersonExistQueryResult
    {
        public bool ExistingPerson { get; set; }

        public Guid PersonID { get; set; }
    }
}
