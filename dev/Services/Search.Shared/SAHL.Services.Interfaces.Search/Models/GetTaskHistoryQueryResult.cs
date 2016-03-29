using System;

namespace SAHL.Services.Interfaces.Search.Models
{
    public class GetTaskHistoryQueryResult
    {
        public string FromState { get; set; }

        public string ToState { get; set; }

        public string Activity { get; set; }

        public string ActivityUser { get; set; }

        public DateTime ActivityDate { get; set; }
    }
}