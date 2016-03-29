using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree.Statements
{
    public class TrimMRUTreeDataForUserQuery : ISqlStatement<UserMRUDecisionTreeDataModel>
    {
        public string UserName { get; protected set; }

        public TrimMRUTreeDataForUserQuery(string username)
        {
            this.UserName = username;
        }

        // we use 10 as a max number of recently used history to allow a max of 5 pinned plus 5 unpinned to be shown on the client
        public string GetStatement()
        {
            return @"create table #mrus(mruid uniqueidentifier)
            insert into #mrus select top 10 id FROM [DecisionTree].[dbo].[UserMRUDecisionTree] where username = @UserName order by username, pinned desc, modifieddate desc
            delete FROM [DecisionTree].[dbo].[UserMRUDecisionTree] where not exists (select mruid from #mrus where mruid = id)
            drop table #mrus";
        }
    }
}