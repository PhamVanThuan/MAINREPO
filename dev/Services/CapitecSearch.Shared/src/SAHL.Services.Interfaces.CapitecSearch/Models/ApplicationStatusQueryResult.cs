using SAHL.Core.TextSearch;

namespace SAHL.Services.Interfaces.CapitecSearch.Models
{
    public class ApplicationStatusQueryResult : IQueryResultModel
    {
        public int DocumentId { get; set; }

        public float Score { get; set; }

        public int ApplicationNumber { get; set; }

        public string ApplicationDate { get; set; }

        public string ApplicationStage { get; set; }

        public string ApplicationStatus { get; set; }

        public string ApplicantsJson { get; set; }

        public string ConsultantName { get; set; }

        public string ConsultantContactNumber { get; set; }

        public string IdentityNumberList { get; set; }
    }
}