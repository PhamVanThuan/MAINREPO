using SAHL.Core.Data;
using SAHL.Services.DocumentManager.Managers.Document.Models;

namespace SAHL.Services.DocumentManager.Managers.Document.Statements
{
    public class FindDocumentStoreByIdStatement : ISqlStatement<DocumentStoreModel>
    {
        public int DocumentStoreId { get; protected set; }

        public FindDocumentStoreByIdStatement(int documentStoreId)
        {
            this.DocumentStoreId = documentStoreId;
        }

        public string GetStatement()
        {
            return @"select [ID]
                          ,[Name]
                          ,[Description]
                          ,[Folder]
                          ,[NonIndexableChars]
                          ,[DefaultDocTitle]
                      FROM [ImageIndex].[dbo].[STOR]
                      WHERE ID = @DocumentStoreId";
        }
    }
}