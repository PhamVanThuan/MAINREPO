using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DeleteVersionQuery : ISqlStatement<DecisionTreeVersionDataModel>
    {
        public Guid DecisionTreeVersionId { get; protected set; }

        public DeleteVersionQuery(Guid decisionTreeVersionId)
        {
            this.DecisionTreeVersionId = decisionTreeVersionId;
        }

        public string GetStatement()
        {
            return "DELETE FROM [DecisionTree].[dbo].[DecisionTreeVersion] WHERE Id = @DecisionTreeVersionId";
        }
    }
}