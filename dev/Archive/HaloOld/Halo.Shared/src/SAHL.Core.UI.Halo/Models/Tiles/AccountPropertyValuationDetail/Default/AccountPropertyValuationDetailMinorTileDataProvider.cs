using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
namespace SAHL.Core.UI.Halo.Tiles.AccountPropertyValuationDetail.Default
{
    public class AccountPropertyValuationDetailMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<AccountPropertyValuationDetailMinorTileModel>
    {
        public AccountPropertyValuationDetailMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT 
                                   v.ValuationKey AS BusinessKey, 
                                   0 AS BusinessKeyType
                                   FROM [2am].dbo.Valuation v (NOLOCK)
                                   JOIN [2am].dbo.Property prop (NOLOCK) on v.PropertyKey = prop.PropertyKey
                                   JOIN [2am].fin.MortgageLoan ml (NOLOCK) on ml.PropertyKey = prop.PropertyKey
                                   JOIN [2am].dbo.FinancialService fs (NOLOCK) on ml.FinancialServiceKey = fs.FinancialServiceKey and fs.FinancialServiceTypeKey = 1
                                   WHERE fs.AccountKey = {0}
                                   AND v.IsActive = 1", businessKey.Key);
        }
    }
}