using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetDecisionTreeByNameAndVersionNumberQueryResult
    {
        public Guid DecisionTreeId { get; set; }

        public Guid DecisionTreeVersionId { get; set; }

        public string Data { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }
    }
}
