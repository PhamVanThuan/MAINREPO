using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.AccountsWithDefaultedPayments.Default
{
    public class AccountsWithDefaultedPaymentsMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<AccountsWithDefaultedPaymentsMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT r.LegalEntityKey AS BusinessKey, 1 AS BusinessKeyType
                                   FROM
                                        [2AM].dbo.Role r (NOLOCK)
                                        LEFT JOIN 
                                        [X2].[X2].[Instance] i (NOLOCK) ON r.AccountKey = i.Name
                                   WHERE 
                                        WorkFlowID = 927
                                   AND 
                                        StateID = 21178
                                   AND 
                                        r.LegalEntityKey = {0}
                                   GROUP BY 
                                        r.LegalEntityKey", businessKey.Key);
        }
    }
}