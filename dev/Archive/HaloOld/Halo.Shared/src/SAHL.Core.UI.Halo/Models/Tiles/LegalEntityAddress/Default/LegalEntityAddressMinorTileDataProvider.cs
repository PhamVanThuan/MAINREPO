using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
namespace SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default
{
    public class LegalEntityAddressMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<LegalEntityAddressMinorTileModel>
    {
        public LegalEntityAddressMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"
                        SELECT LegalEntityAddressKey as BusinessKey, 0 as BusinessKeyType 
                        FROM [2AM].[dbo].[LegalEntityAddress] lea 
                        WHERE 
                        lea.GeneralStatusKey = 1
                        AND lea.LegalEntityKey = {0}", businessKey.Key);
        }
    }
}