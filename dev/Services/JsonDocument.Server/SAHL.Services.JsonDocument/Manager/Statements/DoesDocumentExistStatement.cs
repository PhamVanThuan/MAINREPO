using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.JsonDocument.Managers.Statements
{
    public class DoesDocumentExistStatement : ISqlStatement<JsonDocumentDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesDocumentExistStatement(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return @"SELECT *
                    FROM [doc].[JsonDocument] doc 
                    WHERE doc.id = @Id;";

        }
    }
}
