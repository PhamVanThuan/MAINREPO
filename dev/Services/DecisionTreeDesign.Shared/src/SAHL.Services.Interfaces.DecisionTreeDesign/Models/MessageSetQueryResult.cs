using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class MessageSetQueryResult
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string Data { get; set; }

        public bool IsPublished { get; set; }

        public string LockedBy { get; set; }
    }
}