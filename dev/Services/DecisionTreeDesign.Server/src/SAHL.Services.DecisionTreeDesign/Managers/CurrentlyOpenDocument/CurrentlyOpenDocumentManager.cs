using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument
{
    public class CurrentlyOpenDocumentManager : ICurrentlyOpenDocumentManager
    {
        ICurrentlyOpenDocumentDataManager currentlyOpenDocumentDataManager;

        public CurrentlyOpenDocumentManager(ICurrentlyOpenDocumentDataManager currentlyOpenDocumentDataManager)
        {
            this.currentlyOpenDocumentDataManager = currentlyOpenDocumentDataManager;
        }

        public void OpenDocument(Guid documentVersionId, Guid documentTypeId, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username cannot be null");
            }

            if (currentlyOpenDocumentDataManager.IsDocumentOpen(documentVersionId))
            {
                if (currentlyOpenDocumentDataManager.IsDocumentOpenByUser(documentVersionId, username))
                {
                    currentlyOpenDocumentDataManager.UpdateDocumentOpenDate(documentVersionId, username);
                }
            }
            else
            {
                currentlyOpenDocumentDataManager.OpenDocument(documentVersionId, documentTypeId, username);
            }
        }

        public void CloseDocument(Guid documentVersionId, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username cannot be null");
            }

            if (currentlyOpenDocumentDataManager.IsDocumentOpen(documentVersionId))
            {
                if (currentlyOpenDocumentDataManager.IsDocumentOpenByUser(documentVersionId, username))
                {
                    currentlyOpenDocumentDataManager.CloseDocument(documentVersionId, username);
                }
            }
        }

        public void CloseDocument(Guid documentVersionId)
        {
            currentlyOpenDocumentDataManager.CloseDocument(documentVersionId);
        }


        public IEnumerable<OpenDocumentsView> GetAllOpenDocuments()
        {
            return this.currentlyOpenDocumentDataManager.GetAllOpenDocuments(); 
        }
    }
}
