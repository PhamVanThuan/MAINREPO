using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.DataStore;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using System;
using System.Linq;

namespace SAHL.Services.DocumentManager.CommandHandlers
{
    public class StoreAttorneyInvoiceCommandHandler : IServiceCommandHandler<StoreAttorneyInvoiceCommand>
    {
        private IDocumentDataManager dataManager;
        private IDocumentFileManager documentFileManager;
        private IDataStoreUtils dataStoreUtils;
        private IUnitOfWorkFactory uowFactory;

        public StoreAttorneyInvoiceCommandHandler(IDocumentDataManager dataManager, IDocumentFileManager documentFileManager, IDataStoreUtils dataStoreUtils, IUnitOfWorkFactory uowFactory)
        {
            this.dataManager = dataManager;
            this.documentFileManager = documentFileManager;
            this.dataStoreUtils = dataStoreUtils;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(StoreAttorneyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var documentStore = dataManager.GetDocumentStore(command.StoreId).First();
            var invoice = command.AttorneyInvoiceDocument;
            string dataStoreGuid = dataStoreUtils.GenerateDataStoreGuid();
            var dateArchived = DateTime.Now;
            dataManager.SaveAttorneyInvoiceFile(invoice, command.ThirdPartyInvoiceKey, dataStoreGuid, invoice.InvoiceFileName, command.StoreId, dateArchived);
            var file = Convert.FromBase64String(invoice.FileContentAsBase64);
            messages.Aggregate(documentFileManager.WriteFileToDatedFolder(file, dataStoreGuid, documentStore.Folder, metadata.UserName, DateTime.Now));
            return messages;
        }
    }
}