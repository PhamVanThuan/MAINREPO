using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Common.BankAccount;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail.BankAccount
{
    public class BankAccountChildTileContentDataProvider : HaloTileBaseContentDataProvider<BankAccountChildModel>
    {
        public BankAccountChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
                ba.AccountName,
                ba.AccountNumber,
                acbt.ACBTypeDescription as AccountType,
                acb.ACBBankDescription as Bank,
                acb.ACBBankCode as BankID,
                acbr.ACBBranchDescription as Branch,
                acbr.ACBBranchCode as BranchCode
            from
                [2am].[dbo].[BankAccount] ba
            join
                [2am].[dbo].[ACBBranch] acbr on acbr.ACBBranchCode=ba.ACBBranchCode
            join
                [2am].[dbo].[ACBBank] acb on acb.ACBBankCode=acbr.ACBBankCode
            join
                [2am].[dbo].[ACBType] acbt on acbt.ACBTypeNumber=ba.ACBTypeNumber
            where
                ba.BankAccountKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}