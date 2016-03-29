using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesUserBelongToAnyBranchesQuery : ISqlStatement<UserBranchDataModel>
    {
        public Guid UserId { get; protected set; }

        public DoesUserBelongToAnyBranchesQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public string GetStatement()
        {
            return @"SELECT [UserId],[BranchId]
FROM [Capitec].[security].[UserBranch]
WHERE [UserId] = @UserId";
        }
    }
}