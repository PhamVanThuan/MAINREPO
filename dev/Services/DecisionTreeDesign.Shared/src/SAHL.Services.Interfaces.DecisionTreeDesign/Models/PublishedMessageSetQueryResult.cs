using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class PublishedMessageSetQueryResult
    {
        public Guid MessageSetId { get; set; }

        public int Version { get; set; }

        public string Data { get; set; }

        public Guid PublishedMessageSetId { get; set; }

        public DateTime PublishDate { get; set; }

        public string Publisher { get; set; }

        public string PublishStatus { get; set; }
    }
}