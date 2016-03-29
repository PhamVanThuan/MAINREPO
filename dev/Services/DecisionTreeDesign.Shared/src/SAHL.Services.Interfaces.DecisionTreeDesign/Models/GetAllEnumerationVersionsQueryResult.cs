using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetAllEnumerationVersionsQueryResult
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishDate { get; set; }

        public string Publisher { get; set; }
    }
}