using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetLatestDecisionTreesPubOnlyQueryResult
    {
        public Guid DecisionTreeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Thumbnail { get; set; }

        public Guid DecisionTreeVersionId { get; set; }

        public int LatestVersion { get; set; }

        public bool IsPublished { get; set; }
    }
}
