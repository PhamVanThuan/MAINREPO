using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.Address.Default
{
    public class LegalEntityAddressMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<LegalEntityAddressMinorTileModel>
    {
        public LegalEntityAddressMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format("select lea.addresskey as BusinessKey, 0 as BusinessKeyType from [2am].dbo.legalentityaddress lea where lea.legalentitykey = {0}", businessKey.Key);
        }
    }
}