using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument
{
    public interface ICurrentlyOpenDocumentManager
    {
        void OpenDocument(Guid documentVersionId,Guid documentTypeId, string username);
        void CloseDocument(Guid documentVersionId, string username);
        void CloseDocument(Guid documentVersionId);
        IEnumerable<OpenDocumentsView> GetAllOpenDocuments();
    }
}
