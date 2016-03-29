using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetDecisionTreeByNameQueryResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}