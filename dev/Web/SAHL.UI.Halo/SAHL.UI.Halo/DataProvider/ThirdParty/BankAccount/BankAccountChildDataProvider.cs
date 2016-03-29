using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.BankAccount;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.BankAccount
{
    public class BankAccountChildDataProvider : HaloTileBaseChildDataProvider,
                                                IHaloTileChildDataProvider<BankAccountChildTileConfiguration>
    {
        public BankAccountChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
                                    tppb.BankAccountKey as BusinessKey, 0 as BusinessKeyType
                                from
                                    [2AM].[dbo].[ThirdPartyPaymentBankAccount] tppb
                                where
                                    tppb.ThirdPartyKey = (SELECT [ThirdPartyKey]
                                      FROM [2AM].[dbo].[ThirdParty] tp
                                      where tp.LegalEntityKey = {0}
                                        and tppb.GeneralStatusKey = 1)", businessContext.BusinessKey.Key);
        }
    }
}