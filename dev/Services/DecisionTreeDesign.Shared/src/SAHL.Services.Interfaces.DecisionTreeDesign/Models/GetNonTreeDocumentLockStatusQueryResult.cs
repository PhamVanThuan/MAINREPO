using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetNonTreeDocumentLockStatusQueryResult
    {
        public string DocumentType { get; set; }
        public string Username { get; set; }
    }
}
