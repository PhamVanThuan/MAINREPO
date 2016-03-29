using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument
{
    public class CurrentlyOpenDocumentDataManager : ICurrentlyOpenDocumentDataManager
    {
        public bool IsDocumentOpen(Guid documentVersionId)
        {
            bool result = false;
            IsDocumentOpenQuery query = new IsDocumentOpenQuery(documentVersionId);
            using (var db = new Db().InReadOnlyAppContext())
            {
                CurrentlyOpenDocumentDataModel model = db.SelectOne(query);
                if (model != null)
                {
                    result = true;
                }
            }
            return result;
        }
        
        public bool IsDocumentOpenByUser(Guid documentVersionId, string username)
        {
            bool result = false;
            IsDocumentOpenByUserQuery query = new IsDocumentOpenByUserQuery(documentVersionId, username);
            using (var db = new Db().InReadOnlyAppContext())
            {
                CurrentlyOpenDocumentDataModel model = db.SelectOne(query);
                if (model != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public void OpenDocument(Guid documentVersionId, Guid DocumentTypeId, string username)
        {
            CurrentlyOpenDocumentDataModel model = new CurrentlyOpenDocumentDataModel(CombGuid.Instance.Generate(), documentVersionId, username, DateTime.Now, DocumentTypeId);
            using (var db = new Db().InAppContext())
            {
                db.Insert(model);
                db.Complete();
            }
        }

        public void UpdateDocumentOpenDate(Guid documentVersionId, string username)
        {
            UpdateDocumentOpenDateQuery query = new UpdateDocumentOpenDateQuery(documentVersionId,username);
            using (var db = new Db().InAppContext())
            {
                db.Update<CurrentlyOpenDocumentDataModel>(query);
                db.Complete();
            }
        }

        public void CloseDocument(Guid documentVersionId, string username)
        {
            CloseDocumentQuery query = new CloseDocumentQuery(documentVersionId, username);
            using (var db = new Db().InAppContext())
            {
                db.Delete<CurrentlyOpenDocumentDataModel>(query);
                db.Complete();
            }
        }

        public void CloseDocument(Guid documentVersionId)
        {
            CloseDocumentOverrideQuery query = new CloseDocumentOverrideQuery(documentVersionId);
            using (var db = new Db().InAppContext())
            {
                db.Delete<CurrentlyOpenDocumentDataModel>(query);
                db.Complete();
            }
        }


        public IEnumerable<OpenDocumentsView> GetAllOpenDocuments()
        {
            GetAllOpenDocumentsQuery query = new GetAllOpenDocumentsQuery();
            IEnumerable<OpenDocumentsView> results;
            using (var db = new Db().InAppContext())
            {
                results = db.Select<OpenDocumentsView>(query);
            }
            return results;
        }
    }
}
