using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetLatestDecisionTreesQueryResult
    {
        public Guid DecisionTreeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Thumbnail { get; set; }

        public Guid DecisionTreeVersionId { get; set; }

        public int ThisVersion { get; set; }

        public bool IsPublished { get; set; }

        public string CurrentlyOpenBy { get; set; }
        // Ed made me do it
        //public TreeVersion[] AvailableVersions { get; set; }
    }
}