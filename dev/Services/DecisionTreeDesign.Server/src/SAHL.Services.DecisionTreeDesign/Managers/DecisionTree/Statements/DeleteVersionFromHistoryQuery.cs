using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DeleteVersionFromHistoryQuery : ISqlStatement<DecisionTreeHistoryDataModel>
    {
        public Guid DecisionTreeVersionId { get; protected set; }

        public DeleteVersionFromHistoryQuery(Guid decisionTreeVersionId)
        {
            this.DecisionTreeVersionId = decisionTreeVersionId;
        }

        public string GetStatement()
        {
            return "DELETE FROM [DecisionTree].[dbo].[DecisionTreeHistory] WHERE DecisionTreeVersionId = @DecisionTreeVersionId";
        }
    }
}