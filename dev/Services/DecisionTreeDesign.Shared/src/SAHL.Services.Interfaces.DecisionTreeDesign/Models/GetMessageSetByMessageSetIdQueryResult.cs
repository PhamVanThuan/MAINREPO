using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetMessageSetByMessageSetIdQueryResult
    {
        public Guid MessageSetId { get; set; }

        public int Version { get; set; }

        public string Data { get; set; }
    }
}
