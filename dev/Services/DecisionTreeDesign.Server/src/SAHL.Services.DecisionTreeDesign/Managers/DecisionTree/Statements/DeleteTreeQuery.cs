using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DeleteTreeQuery : ISqlStatement<DecisionTreeDataModel>
    {
        public Guid DecisionTreeId { get; protected set; }

        public DeleteTreeQuery(Guid decisionTreeId)
        {
            this.DecisionTreeId = decisionTreeId;
        }

        public string GetStatement()
        {
            return "DELETE FROM [DecisionTree].[dbo].[DecisionTree] WHERE Id = @DecisionTreeId";
        }
    }
}