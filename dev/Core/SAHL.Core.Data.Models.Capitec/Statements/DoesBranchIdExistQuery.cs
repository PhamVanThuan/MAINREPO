using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesBranchIdExistQuery : ISqlStatement<BranchDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesBranchIdExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return @"SELECT Id,BranchName,SuburbId,IsActive,BranchCode FROM [Capitec].[security].[Branch] WHERE Id = @Id";
        }
    }
}