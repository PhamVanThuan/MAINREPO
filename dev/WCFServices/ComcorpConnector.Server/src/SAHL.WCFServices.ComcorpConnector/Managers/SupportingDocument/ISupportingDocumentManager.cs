using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument
{
    public interface ISupportingDocumentManager
    {
        SupportingDocumentsReply SaveClientFileDocuments(List<ApplicationDocumentsModel> applicationDocuments, headerType requestHeader);
    }
}