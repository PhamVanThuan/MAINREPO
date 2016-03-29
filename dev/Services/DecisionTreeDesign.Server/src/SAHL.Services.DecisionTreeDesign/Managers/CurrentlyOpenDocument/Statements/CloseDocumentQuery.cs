using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements
{
    public class CloseDocumentQuery : ISqlStatement<CurrentlyOpenDocumentDataModel>
    {
        public Guid DocumentVersionId { get; protected set; }
        public string Username { get; protected set; }

        public CloseDocumentQuery(Guid documentVersionId,string username)
        {
            this.DocumentVersionId = documentVersionId;
            this.Username = username;
        }

        public string GetStatement()
        {
            return @"DELETE FROM [DecisionTree].[dbo].[CurrentlyOpenDocument]
                    WHERE [DocumentVersionId] = @DocumentVersionId AND [Username] = @Username";
        }
    }
}
