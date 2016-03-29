using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetVariableSetByVariableSetIdQueryResult
    {
        public Guid ID { get; set; }

        public int Version { get; set; }

        public string Data { get; set; }

        public bool IsPublished { get; set; }

        public string Publisher { get; set; }
    }
}
