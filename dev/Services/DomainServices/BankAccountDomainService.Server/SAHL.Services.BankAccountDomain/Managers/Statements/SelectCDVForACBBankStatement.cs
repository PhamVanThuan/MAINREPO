using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class SelectCDVForACBBankStatement : ISqlStatement<CDVDataModel>
    {
        public int ACBBankCode { get; protected set; }

        public string ACBBranchCode { get; protected set; }

        public int ACBTypeNumber { get; protected set; }

        public SelectCDVForACBBankStatement(int acbBankCode, string acbBranchCode, int acbTypeNumber)
        {
            this.ACBBankCode = acbBankCode;
            this.ACBBranchCode = acbBranchCode;
            this.ACBTypeNumber = acbTypeNumber;
        }

        public string GetStatement()
        {
            return @"SELECT [CDVKey]
                            ,[ACBBankCode]
                            ,[ACBBranchCode]
                            ,[ACBTypeNumber]
                            ,[StreamCode]
                            ,[ExceptionStreamCode]
                            ,[Weightings]
                            ,[Modulus]
                            ,[FudgeFactor]
                            ,[ExceptionCode]
                            ,[AccountIndicator]
                            ,[UserID]
                            ,[DateChange]
                        FROM [2AM].[dbo].[CDV]
                        Where ACBBankCode = @ACBBankCode
                        And ACBBranchCode = @ACBBranchCode
                        And ACBTypeNumber = @ACBTypeNumber";
        }
    }
}