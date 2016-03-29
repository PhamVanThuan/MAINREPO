using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree.Statements
{
    public class RemoveDecisionTreeMRUForUserQuery : ISqlStatement<UserMRUDecisionTreeDataModel>
    {
        public string UserName { get; protected set; }

        public Guid TreeId { get; protected set; }

        public RemoveDecisionTreeMRUForUserQuery(Guid treeId, string username)
        {
            this.UserName = username;
            this.TreeId = treeId;
        }

        public string GetStatement()
        {
            return "DELETE FROM [DecisionTree].[dbo].[UserMRUDecisionTree] WHERE TreeId = @TreeId and UserName = @UserName";
        }
    }
}