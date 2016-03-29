using SAHL.Core.Data;
using SAHL.Services.DocumentManager.Managers.Document.Models;
using SAHL.Services.DocumentManager.Managers.Document.Statements;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DocumentManager.Managers.Document
{
    public class DocumentDataManager : IDocumentDataManager
    {
        private IDbFactory dbFactory;

        public DocumentDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<DocumentStoreModel> GetDocumentStore(int storeId)
        {
            FindDocumentStoreByIdStatement clientQuery = new FindDocumentStoreByIdStatement(storeId);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<DocumentStoreModel>(clientQuery);
            }
        }

        public DocumentStorDocumentInfoModel GetDocumentInfoByGuidAndStoreId(Guid documentGuid, int storeId)
        {
            var getDocumentInfoQuery = new FindDocumentInformationByDocumentIdAndStoreIdStatement(documentGuid.ToString("B"), storeId);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var documentInfoModel = db.SelectOne<DocumentStorDocumentInfoModel>(getDocumentInfoQuery);
                return documentInfoModel;
            }
        }

        public void SaveClientFile(ClientFileDocumentModel document, string dataStoreGuid, string originalFileName, DateTime documentInsertDate)
        {
            string createdDate = documentInsertDate.ToString("dd/MM/yyyy hh:mm tt");
            string archiveDate = documentInsertDate.ToString("yyyy-MM-dd HH:mm:ss");
            string username = document.Username.Replace("SAHL\\", String.Empty);
            int clientFileStoreId = Convert.ToInt32(DocumentStorEnum.Clientfiles);

            ISqlStatement<ClientFileDocumentModel> insertClientFile = new InsertClientFileDocumentDataStatement(document, FileExtension.Pdf, dataStoreGuid, originalFileName, username,
                clientFileStoreId, createdDate, archiveDate, documentInsertDate);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Insert(insertClientFile);
                db.Complete();
            }
        }

        public void SaveAttorneyInvoiceFile(ThirdPartyInvoiceDocumentModel invoice, string thirdPartyInvoiceKey, string dataStoreGuid, string originalFileName, int storeId,
            DateTime dateArchived)
        {
            var dateArchivedString = dateArchived.ToString("dd-MMM-yyyy");
            ISqlStatement<ThirdPartyInvoiceDocumentModel> insertAttorneyInvoice = new InsertAttorneyInvoiceDataStatement(invoice, thirdPartyInvoiceKey, dataStoreGuid, 
                originalFileName, storeId, dateArchivedString);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Insert(insertAttorneyInvoice);
                db.Complete();
            }
        }

        public void RemoveAttorneyInvoice(int attorneyInvoiceKey)
        {
            var deleteQuery = new DeleteAttorneyInvoiceDataStatement(attorneyInvoiceKey);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Delete(deleteQuery);
                db.Complete();
            }
        }
    }

}