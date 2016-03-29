using SAHL.Services.DocumentManager.Managers.Document.Models;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DocumentManager.Managers.Document
{
    public interface IDocumentDataManager
    {
        void SaveClientFile(ClientFileDocumentModel document, string dataStoreGuid, string originalFileName, DateTime documentInsertDate);

        IEnumerable<DocumentStoreModel> GetDocumentStore(int storeId);

        DocumentStorDocumentInfoModel GetDocumentInfoByGuidAndStoreId(Guid documentGuid, int storeId);

        void SaveAttorneyInvoiceFile(ThirdPartyInvoiceDocumentModel invoice, string thirdPartyInvoiceKey, string dataStoreGuid, string originalFileName, int storeId, DateTime dateArchived);

        void RemoveAttorneyInvoice(int attorneyInvoiceKey);
    }
}