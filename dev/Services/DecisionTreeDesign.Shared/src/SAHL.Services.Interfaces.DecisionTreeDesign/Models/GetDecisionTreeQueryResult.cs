using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetDecisionTreeQueryResult
    {
        public Guid DecisionTreeId { get; set; }

        public Guid DecisionTreeVersionId { get; set; }

        public string Data { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public int MaxVersion { get; set; }
    }
}