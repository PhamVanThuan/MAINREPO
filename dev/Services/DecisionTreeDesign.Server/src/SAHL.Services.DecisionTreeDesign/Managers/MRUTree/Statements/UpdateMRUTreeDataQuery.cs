using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree.Statements
{
    public class UpdateMRUTreeDataQuery : ISqlStatement<UserMRUDecisionTreeDataModel>
    {
        public string UserName { get; protected set; }

        public Guid TreeId { get; protected set; }

        public UpdateMRUTreeDataQuery(Guid treeId, string username)
        {
            this.UserName = username;
            this.TreeId = treeId;
        }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[UserMRUDecisionTree] SET ModifiedDate = getdate() WHERE TreeId = @TreeId and Username =  @UserName";
        }
    }
}