using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument
{
    public interface ICurrentlyOpenDocumentDataManager
    {
        bool IsDocumentOpen(Guid documentVersionId);
        bool IsDocumentOpenByUser(Guid documentVersionId,string username);

        void OpenDocument(Guid documentVersionId, Guid DocumentTypeId, string username);
        void UpdateDocumentOpenDate(Guid documentVersionId, string username);

        void CloseDocument(Guid documentVersionId, string username);
        void CloseDocument(Guid documentVersionId);

        IEnumerable<OpenDocumentsView> GetAllOpenDocuments();
    }
}
