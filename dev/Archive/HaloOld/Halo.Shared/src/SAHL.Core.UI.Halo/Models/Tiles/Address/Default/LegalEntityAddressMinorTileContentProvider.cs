using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.Address.Default
{
    public class LegalEntityAddressMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityAddressMinorTileModel>
    {
        public override string GetStatement(BusinessModel.BusinessKey businessKey)
        {
            return string.Format("SELECT [2AM].[dbo].[fGetFormattedAddressDelimited] ({0}, 0) as AddressSummary", businessKey.Key);
        }
    }
}