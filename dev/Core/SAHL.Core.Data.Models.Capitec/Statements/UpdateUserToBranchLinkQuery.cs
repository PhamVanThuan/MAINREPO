using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UpdateUserToBranchLinkQuery : ISqlStatement<UserBranchDataModel>
    {
        public Guid UserId { get; protected set; }

        public Guid BranchId { get; protected set; }

        public UpdateUserToBranchLinkQuery(Guid userId, Guid branchId)
        {
            this.UserId = userId;
            this.BranchId = branchId;
        }

        public string GetStatement()
        {
            return @"UPDATE [Capitec].[security].[UserBranch]
SET [BranchId] = @BranchId
WHERE [UserId] = @UserId";
        }
    }
}