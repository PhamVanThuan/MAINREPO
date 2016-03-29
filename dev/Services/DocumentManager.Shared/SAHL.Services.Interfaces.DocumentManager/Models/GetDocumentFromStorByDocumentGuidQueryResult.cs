
namespace SAHL.Services.Interfaces.DocumentManager.Models
{
    public class GetDocumentFromStorByDocumentGuidQueryResult
    {
        public GetDocumentFromStorByDocumentGuidQueryResult()
        {
        }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string FileContentAsBase64 { get; set; }
    }
}
