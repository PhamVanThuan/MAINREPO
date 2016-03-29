using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.AccountPropertyValuationDetail.Default
{
    public class AccountPropertyValuationDetailMinorTileContentProvider : AbstractSqlTileContentProvider<AccountPropertyValuationDetailMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT
                                   v.ValuationDate AS LastValuationDate,
                                   v.ValuationAmount AS LastValuationAmount
                                   FROM Valuation v 
                                   WHERE v.ValuationKey = {0}", businessKey.Key);
        }
    }
}