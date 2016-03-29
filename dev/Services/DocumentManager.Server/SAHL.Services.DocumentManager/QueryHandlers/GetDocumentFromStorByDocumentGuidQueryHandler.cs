using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DocumentManager.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DocumentManager.QueryHandlers
{
    public class GetDocumentFromStorByDocumentGuidQueryHandler : IServiceQueryHandler<GetDocumentFromStorByDocumentGuidQuery>
    {
        private IDocumentDataManager documentDataManager;
        private IDocumentFileManager documentFileManager;


        public GetDocumentFromStorByDocumentGuidQueryHandler(IDocumentDataManager documentDataManager, IDocumentFileManager documentFileManager)
        {
            this.documentDataManager = documentDataManager;
            this.documentFileManager = documentFileManager;
        }

        public ISystemMessageCollection HandleQuery(GetDocumentFromStorByDocumentGuidQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            query.Result = new ServiceQueryResult<GetDocumentFromStorByDocumentGuidQueryResult>(
                                new List<GetDocumentFromStorByDocumentGuidQueryResult> { new GetDocumentFromStorByDocumentGuidQueryResult() });

            int storeId = Convert.ToInt32(query.Store);
            var documentStore = documentDataManager.GetDocumentStore(storeId).FirstOrDefault();

            if (documentStore == null)
            {
                messages.AddMessage(new SystemMessage("The specified document store does not exist.", SystemMessageSeverityEnum.Error));
                return messages;
            }

            var documentInfo = documentDataManager.GetDocumentInfoByGuidAndStoreId(query.DocumentGuid, storeId);

            if (documentInfo == null)
            {
                messages.AddMessage(new SystemMessage("The requested document does not exist.", SystemMessageSeverityEnum.Error));
                return messages;
            }

            var fileContentAsBase64 = documentFileManager.ReadFileFromDatedFolderAsBase64(query.DocumentGuid.ToString("B"), documentStore.Folder, documentInfo.ArchiveDate);

            var queryResult = new GetDocumentFromStorByDocumentGuidQueryResult
            {
                FileName = documentInfo.FileName,
                FileExtension = documentInfo.FileExtension,
                FileContentAsBase64 = fileContentAsBase64
            };

            query.Result = new ServiceQueryResult<GetDocumentFromStorByDocumentGuidQueryResult>(new List<GetDocumentFromStorByDocumentGuidQueryResult> { queryResult });

            return messages;
        }
    }
}