using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models
{
    public class ApplicationDocumentsModel
    {
        public string IdentityNumber { get; set; }

        public List<documentType> SupportingDocuments { get; set; }

        public DateTime RequestDateTime { get; set; }

        public ApplicationDocumentsModel(string identityNumber, List<documentType> supportingDocuments, DateTime requestDateTime)
        {
            this.IdentityNumber = identityNumber;
            this.SupportingDocuments = supportingDocuments;
            this.RequestDateTime = requestDateTime;
        }
    }
}