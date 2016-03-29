using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class GetMinimumLoanAmountForApplicationStatement : ISqlStatement<decimal>
    {
        public int MortgageLoanPurposeKey { get; protected set; }

        public GetMinimumLoanAmountForApplicationStatement(int MortgageLoanPurposeKey)
        {
            this.MortgageLoanPurposeKey = MortgageLoanPurposeKey;
        }

        public string GetStatement()
        {
            var sql = @"select cast(isnull(min(cc.MinLoanAmount), 0) as decimal(22, 2)) as MininimLoanAmountRequired
                        from [2AM].[dbo].[CreditMatrix] cm
                        join [2AM].[dbo].[CreditCriteria] cc on cm.CreditMatrixKey = cc.CreditMatrixKey
                               and cc.MortgageLoanPurposeKey = @MortgageLoanPurposeKey
                        join [2AM].[dbo].[OriginationSourceProductCreditMatrix] ospcm on cm.CreditMatrixKey = ospcm.CreditMatrixKey
                        join [2AM].[dbo].[OriginationSourceProduct] osp on ospcm.OriginationSourceProductKey = osp.OriginationSourceProductKey
                               and osp.OriginationSourceKey = 1 --SAHL
                               and osp.ProductKey = 9 -- New Variable Loan
                        where cm.NewBusinessIndicator = 'Y'";

            return sql;
        }
    }
}