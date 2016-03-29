using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree.Statements
{
    public class SaveMRUTreePinStatusDataQuery : ISqlStatement<UserMRUDecisionTreeDataModel>
    {
        public string UserName { get; protected set; }

        public Guid TreeId { get; protected set; }

        public bool Pinned { get; protected set; }

        public SaveMRUTreePinStatusDataQuery(Guid treeId, string username, bool pinned)
        {
            this.UserName = username;
            this.TreeId = treeId;
            this.Pinned = pinned;
        }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[UserMRUDecisionTree] SET Pinned = @Pinned WHERE TreeId = @TreeId and Username =  @UserName";
        }
    }
}