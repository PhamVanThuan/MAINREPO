using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class SelectACBBankForACBBranchStatement : ISqlStatement<ACBBankDataModel>
    {
        public string ACBBranchCode { get; protected set; }

        public SelectACBBankForACBBranchStatement(string branchCode)
        {
            this.ACBBranchCode = branchCode;
        }

        public string GetStatement()
        {
            return @"SELECT bank.[ACBBankCode],[ACBBankDescription]
                      FROM [2AM].[dbo].[ACBBank] bank
                      JOIN [2AM].[dbo].[ACBBranch] branch
                      ON bank.ACBBankCode = branch.ACBBankCode
                      WHERE branch.ACBBranchCode = @ACBBranchCode";
        }
    }
}