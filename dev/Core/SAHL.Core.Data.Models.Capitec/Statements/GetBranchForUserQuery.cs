using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetBranchForUserQuery : ISqlStatement<BranchDataModel>
    {
        public Guid UserId { get; protected set; }

        public GetBranchForUserQuery(Guid userId)
        {
            this.UserId = userId;
        }
        public string GetStatement()
        {
            return @"SELECT Branch.Id,Branch.BranchName,Branch.SuburbId,Branch.IsActive,Branch.BranchCode FROM [Capitec].[security].[Branch]
                        JOIN [Capitec].[security].[UserBranch] on Branch.Id = UserBranch.BranchId
                        WHERE UserBranch.UserId = @UserId";
        }
    }
}