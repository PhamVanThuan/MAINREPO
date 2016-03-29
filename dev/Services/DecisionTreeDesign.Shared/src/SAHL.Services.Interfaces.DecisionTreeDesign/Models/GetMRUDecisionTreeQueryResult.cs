using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetMRUDecisionTreeQueryResult
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid TreeId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Guid DecisionTreeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Thumbnail { get; set; }

        public Guid DecisionTreeVersionId { get; set; }

        public int Version { get; set; }

        public bool IsPublished { get; set; }

        public bool Pinned { get; set; }

        public string CurrentlyOpenBy { get; set; }
    }
}