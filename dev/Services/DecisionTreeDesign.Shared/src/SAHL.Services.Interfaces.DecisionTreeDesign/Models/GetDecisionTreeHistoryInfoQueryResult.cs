using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetDecisionTreeHistoryInfoQueryResult
    {
        public Guid Id { get; set; }

        public Guid DecisionTreeVersionId { get; set; }

        public int DecisionTreeVersion { get; set; }

        public bool IsPublished { get; set; }

        public DateTime ModificationDate { get; set; }

        public string ModificationUser { get; set; }
    }
}