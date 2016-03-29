
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DocumentManager.Queries
{
    public class GetDocumentFromStorByDocumentGuidQuery : ServiceQuery<GetDocumentFromStorByDocumentGuidQueryResult>, IDocumentManagerQuery
    {
        [Required(ErrorMessage = "A Store is required.")]
        public DocumentStorEnum Store { get; protected set; }

        [Required(ErrorMessage = "A Document Guid is required.")]
        public Guid DocumentGuid { get; protected set; }

        public GetDocumentFromStorByDocumentGuidQuery(DocumentStorEnum store, Guid documentGuid)
        {
            Store = store;
            DocumentGuid = documentGuid;
        }
    }
}
