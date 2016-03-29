using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class UpdateDecisionTreeDataQuery : ISqlStatement<DecisionTreeVersionDataModel>
    {
        public UpdateDecisionTreeDataQuery(Guid decisionTreeId, string data)
        {
            this.DecisionTreeId = decisionTreeId;
            this.Data = data;
        }

        public Guid DecisionTreeId { get; protected set; }

        public string Data { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[DecisionTreeVersion] SET Data = @Data WHERE Id = @DecisionTreeId";
        }
    }
}