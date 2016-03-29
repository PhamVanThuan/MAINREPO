namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetBranchByNameQuery : ISqlStatement<BranchDataModel>
    {
        public string BranchName { get; protected set; }

        public GetBranchByNameQuery(string branchName)
        {
            BranchName = branchName;
        }

        public string GetStatement()
        {
            return @"SELECT Id,BranchName,SuburbId,IsActive,BranchCode FROM [Capitec].[security].[Branch] WHERE BranchName = @BranchName";
        }
    }
}