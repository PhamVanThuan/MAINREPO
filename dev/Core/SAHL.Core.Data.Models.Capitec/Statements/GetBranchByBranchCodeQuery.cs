namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetBranchByBranchCodeQuery : ISqlStatement<BranchDataModel>
    {
        public string BranchCode { get; protected set; }

        public GetBranchByBranchCodeQuery(string branchCode)
        {
            BranchCode = branchCode;
        }

        public string GetStatement()
        {
            return @"SELECT Id,BranchName,SuburbId,IsActive,BranchCode FROM [Capitec].[security].[Branch] WHERE BranchCode = @BranchCode";
        }
    }
}