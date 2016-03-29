using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements
{
    public class UpdateDocumentOpenDateQuery : ISqlStatement<CurrentlyOpenDocumentDataModel>
    {
        public Guid DocumentVersionId { get; protected set; }
        public string Username { get; protected set; }
        public DateTime NewDate {get;protected set;}

        public UpdateDocumentOpenDateQuery(Guid documentVersionId, string username)
        {
            this.DocumentVersionId = documentVersionId;
            this.Username = username;
            this.NewDate = DateTime.Now;//done to ensure that only the server time is used
        }

        public string GetStatement()
        {
            return @"UPDATE [DecisionTree].[dbo].[CurrentlyOpenDocument]
                    SET [OpenDate] = @NewDate
                    WHERE [DocumentVersionId] = @DocumentVersionId AND [Username] = @Username";
        }
    }
}
