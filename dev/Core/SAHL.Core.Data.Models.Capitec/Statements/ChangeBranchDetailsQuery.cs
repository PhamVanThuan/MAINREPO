using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class ChangeBranchDetailsQuery : ISqlStatement<BranchDataModel>
    {
        public Guid Id { get; protected set; }
        public string BranchName { get; protected set; }
        public bool IsActive { get; protected set; }
        public Guid SuburbId { get; protected set; }

        public ChangeBranchDetailsQuery(Guid id, string branchName, bool isActive, Guid suburbId)
        {
            this.Id = id;
            this.BranchName = branchName;
            this.IsActive = isActive;
            this.SuburbId = suburbId;
        }

        public string GetStatement()
        {
            return @"UPDATE [Capitec].[security].[Branch]
SET [BranchName] = @BranchName,[IsActive] = @IsActive,[SuburbId] = @SuburbId
WHERE Id = @Id";
        }
    }
}