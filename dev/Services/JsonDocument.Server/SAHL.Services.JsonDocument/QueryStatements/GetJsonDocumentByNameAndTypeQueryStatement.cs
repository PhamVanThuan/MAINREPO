﻿using SAHL.Core.Services;
using SAHL.Services.Interfaces.JsonDocument.Query;

namespace SAHL.Services.JsonDocument.QueryStatements
{
    public class GetJsonDocumentByNameAndTypeQueryStatement : IServiceQuerySqlStatement<GetJsonDocumentByNameAndTypeQuery, GetJsonDocumentByNameAndTypeQueryResult>
        
    {
        public string GetStatement()
        {
            return @"SELECT [dbo].[JsonDocument](doc.id,doc.Name,doc.[Description],doc.[Version],doc.DocumentFormatVersion,docType.Name,doc.Data) AS 'JsonDocument'
FROM [2AM].[doc].[JsonDocument] doc
JOIN [2AM].[doc].[JsonDocumentType] docType ON doc.DocumentType = docType.Id
WHERE docType.NAME = @Type
    AND doc.NAME = @Name";
        }
    }
}