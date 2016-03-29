using SAHL.Core.Data;
using SAHL.Services.Interfaces.DocumentManager.Models;

namespace SAHL.Services.DocumentManager.Managers.Document.Statements
{
    public class FindDocumentInformationByDocumentIdAndStoreIdStatement : ISqlStatement<DocumentStorDocumentInfoModel>
    {
        public int StoreId { get; protected set; }
        public string DocumentGuid { get; protected set; }

        public FindDocumentInformationByDocumentIdAndStoreIdStatement(string documentGuid, int storeId)
        {
            StoreId = storeId;
            DocumentGuid = documentGuid;
        }

        public string GetStatement()
        {
            return string.Format(@"SELECT TOP 1
                                        OriginalFileName AS FileName,
                                        Extension AS FileExtension,
                                        archiveDate AS ArchiveDate
                                   FROM 
                                        [ImageIndex].[dbo].[Data] 
                                   WHERE 
                                        GUID = @DocumentGuid
                                    AND 
                                        STOR = @StoreId");
        }
    }
}
