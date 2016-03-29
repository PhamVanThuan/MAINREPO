using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesUserBelongToBranchQuery : ISqlStatement<UserBranchDataModel>
    {
        public Guid UserId { get; protected set; }
        public Guid BranchId { get; protected set; }

        public DoesUserBelongToBranchQuery(Guid userId, Guid branchId)
        {
            this.UserId = userId;
            this.BranchId = branchId;
        }

        public string GetStatement()
        {
            return @"SELECT [UserId],[BranchId]
FROM [Capitec].[security].[UserBranch]
WHERE [UserId] = @UserId AND [BranchId] = @BranchId";
        }
    }
}