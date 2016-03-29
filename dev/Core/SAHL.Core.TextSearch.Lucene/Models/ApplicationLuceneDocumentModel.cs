using SAHL.Core.Services.Attributes;

namespace SAHL.Core.TextSearch.Lucene.Models
{
    public class ApplicationLuceneDocumentModel
    {
        [LuceneFieldUniqueIdAttribute, LuceneFieldAnalyseAttribute]
        public int ApplicationNumber { get; set; }

        [LuceneFieldAnalyseAttribute]
        public string IdentityNumberList { get; set; }

        public string ApplicationDate { get; set; }

        public string ApplicationStage { get; set; }

        public string ApplicationStatus { get; set; }

        public string ApplicantsJson { get; set; }

        public string ConsultantName { get; set; }

        public string ConsultantContactNumber { get; set; }
    }
}