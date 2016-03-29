using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class GetBranchStatement : ISqlStatement<ACBBranchDataModel>
    {
        public string BranchCode { get; protected set; }
        public GetBranchStatement(string branchCode)
        {
            this.BranchCode = branchCode;
        }

        public string GetStatement()
        {
            var query = @"SELECT
                           [ACBBranchCode]
                          ,[ACBBankCode]
                          ,[ACBBranchDescription]
                          ,[ActiveIndicator]
                      FROM [ACBBranch]
                      WHERE 
                           [ACBBranchCode] = @BranchCode";
            return query;
        }
    }
}
