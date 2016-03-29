using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string jsondocumenttypedatamodel_selectwhere = "SELECT Id, Name FROM [2AM].[doc].[JsonDocumentType] WHERE";
        public const string jsondocumenttypedatamodel_selectbykey = "SELECT Id, Name FROM [2AM].[doc].[JsonDocumentType] WHERE Id = @PrimaryKey";
        public const string jsondocumenttypedatamodel_delete = "DELETE FROM [2AM].[doc].[JsonDocumentType] WHERE Id = @PrimaryKey";
        public const string jsondocumenttypedatamodel_deletewhere = "DELETE FROM [2AM].[doc].[JsonDocumentType] WHERE";
        public const string jsondocumenttypedatamodel_insert = "INSERT INTO [2AM].[doc].[JsonDocumentType] (Id, Name) VALUES(@Id, @Name); ";
        public const string jsondocumenttypedatamodel_update = "UPDATE [2AM].[doc].[JsonDocumentType] SET Id = @Id, Name = @Name WHERE Id = @Id";



        public const string jsondocumentdatamodel_selectwhere = "SELECT Id, Name, Description, Version, DocumentFormatVersion, DocumentType, Data FROM [2AM].[doc].[JsonDocument] WHERE";
        public const string jsondocumentdatamodel_selectbykey = "SELECT Id, Name, Description, Version, DocumentFormatVersion, DocumentType, Data FROM [2AM].[doc].[JsonDocument] WHERE Id = @PrimaryKey";
        public const string jsondocumentdatamodel_delete = "DELETE FROM [2AM].[doc].[JsonDocument] WHERE Id = @PrimaryKey";
        public const string jsondocumentdatamodel_deletewhere = "DELETE FROM [2AM].[doc].[JsonDocument] WHERE";
        public const string jsondocumentdatamodel_insert = "INSERT INTO [2AM].[doc].[JsonDocument] (Id, Name, Description, Version, DocumentFormatVersion, DocumentType, Data) VALUES(@Id, @Name, @Description, @Version, @DocumentFormatVersion, @DocumentType, @Data); ";
        public const string jsondocumentdatamodel_update = "UPDATE [2AM].[doc].[JsonDocument] SET Id = @Id, Name = @Name, Description = @Description, Version = @Version, DocumentFormatVersion = @DocumentFormatVersion, DocumentType = @DocumentType, Data = @Data WHERE Id = @Id";



    }
}