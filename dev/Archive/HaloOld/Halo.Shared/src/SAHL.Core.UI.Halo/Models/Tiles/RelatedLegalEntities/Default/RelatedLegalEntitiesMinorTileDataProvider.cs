using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.RelatedLegalEntities.Default
{
    public class RelatedLegalEntitiesMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<RelatedLegalEntitiesMinorTileModel>
    {
        public RelatedLegalEntitiesMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT ler.LegalEntityKey as BusinessKey,1 as BusinessTypeKey
                                    FROM LegalEntityRelationship ler (NOLOCK)
                                    WHERE ler.LegalEntityKey = {0} and RelationshipTypeKey <> 5
                                    GROUP BY ler.LegalEntityKey", businessKey.Key);
        }
    }
}