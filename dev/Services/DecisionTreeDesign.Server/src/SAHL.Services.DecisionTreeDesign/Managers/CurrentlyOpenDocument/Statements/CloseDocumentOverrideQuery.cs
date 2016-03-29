using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements
{
    public class CloseDocumentOverrideQuery : ISqlStatement<CurrentlyOpenDocumentDataModel>
    {
        public Guid DocumentVersionId { get; protected set; }

        public CloseDocumentOverrideQuery(Guid documentVerisonId)
        {
            this.DocumentVersionId = documentVerisonId;
        }

        public string GetStatement()
        {
            return @"DELETE FROM [DecisionTree].[dbo].[CurrentlyOpenDocument]
                    WHERE [DocumentVersionId] = @DocumentVersionId";
        }
    }
}
