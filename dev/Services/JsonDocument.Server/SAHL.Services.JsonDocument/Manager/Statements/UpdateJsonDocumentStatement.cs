using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.JsonDocument.Managers.Statements
{
    public class UpdateJsonDocumentStatement : ISqlStatement<JsonDocumentDataModel>
    {
        public UpdateJsonDocumentStatement(Guid id, string name, string description, int version, string documentFormatVersion, string documentType, string data)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Version = version;
            this.DocumentFormatVersion = documentFormatVersion;
            this.DocumentType = documentType;
            this.Data = data;

        }
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public int Version { get; protected set; }

        public string DocumentFormatVersion { get; protected set; }

        public string DocumentType { get; protected set; }

        public string Data { get; protected set; }

        public string GetStatement()
        {
            return @"UPDATE [doc].[JsonDocument]
       SET [Name] = @Name
          ,[Description] = @Description
          ,[Version] = @Version
          ,[DocumentFormatVersion] = @DocumentFormatVersion
          ,[DocumentType] = docType.Id
          ,[Data] = @Data
    FROM [doc].[JsonDocument] doc inner join [doc].[JsonDocumentType] docType on docType.Name = @DocumentType
    WHERE doc.id = @Id;";

        }
    }
}
