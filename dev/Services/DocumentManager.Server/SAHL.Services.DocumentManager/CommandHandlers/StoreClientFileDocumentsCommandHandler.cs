using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.DataStore;
using SAHL.Services.DocumentManager.Utils.PdfConverter;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Linq;

namespace SAHL.Services.DocumentManager.CommandHandlers
{
    public class StoreClientFileDocumentsCommandHandler : IServiceCommandHandler<StoreClientFileDocumentsCommand>
    {
        private IDocumentDataManager dataManager;
        private IDocumentFileManager documentFileManager;
        private IDataStoreUtils dataStoreUtils;
        private IUnitOfWorkFactory uowFactory;
        private IPdfConverter pdfConverter;
        private readonly int clientFilesPdfVersion = 17;

        public StoreClientFileDocumentsCommandHandler(IDocumentDataManager dataManager, IDocumentFileManager documentFileManager, IDataStoreUtils dataStoreUtils,
                                                        IPdfConverter pdfConverter, IUnitOfWorkFactory uowFactory)
        {
            this.dataManager = dataManager;
            this.documentFileManager = documentFileManager;
            this.dataStoreUtils = dataStoreUtils;
            this.pdfConverter = pdfConverter;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(StoreClientFileDocumentsCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var storeId = Convert.ToInt32(DocumentStorEnum.Clientfiles);
            var clientFileStore = dataManager.GetDocumentStore(storeId).First();
            foreach (var document in command.ClientFileDocuments)
            {
                var documentInsertDate = DateTime.Now;
                string originalFileName = dataStoreUtils.GetFileNameForClientFileDocument(document, documentInsertDate);
                string dataStoreGuid = dataStoreUtils.GenerateDataStoreGuid();

                dataManager.SaveClientFile(document, dataStoreGuid, originalFileName, documentInsertDate);

                var file = Convert.FromBase64String(document.Document);
                if (document.OriginalExtension != FileExtension.Pdf)
                {
                    var convertedFile = pdfConverter.ConvertImageToPdf(file, clientFilesPdfVersion);
                    file = convertedFile;
                }
                messages.Aggregate(documentFileManager.WriteFileToDatedFolder(file, dataStoreGuid, clientFileStore.Folder, document.Username, documentInsertDate));
                if (messages.ErrorMessages().Any())
                {
                    return messages;
                }
            }
            return messages;
        }
    }
}