using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Detail.BankAccount;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail.BankAccount
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
                                    leba.BankAccountKey as BusinessKey, 0 as BusinessKeyType
                                from
                                    [2am].[dbo].[LegalEntityBankAccount] leba
                                where
                                    leba.LegalEntityKey = {0}
                                    and leba.GeneralStatusKey = 1", businessContext.BusinessKey.Key);
        }
    }
}