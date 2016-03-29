using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.PowerOfAttorney.Default
{
    public class LegalEntityPowerOfAttorneyMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<LegalEntityPowerOfAttorneyMinorTileModel>
    {
        public LegalEntityPowerOfAttorneyMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT le.LegalEntityKey as BusinessKey,  1 as BusinessKeyType
                                    from [2AM].[dbo].[LegalEntity] le
                                    join [2AM].[dbo].[LegalEntityRelationship] ler on ler.LegalEntityKey = le.LegalEntityKey and ler.RelationshipTypeKey = 5
                                    where le.legalentitykey = {0}
                                    group by le.LegalEntityKey", businessKey.Key);
        }
    }
}