using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.RelatedLegalEntities.Default
{
    public class RelatedLegalEntitiesMinorTileContentProvider : AbstractSqlTileContentProvider<RelatedLegalEntitiesMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT Count(ler.LegalEntityKey) as NumberOfRelatedLegalEntities
                                    FROM LegalEntityRelationship ler (NOLOCK)
                                    WHERE ler.LegalEntityKey = {0} and RelationshipTypeKey <> 5
                                    GROUP BY ler.LegalEntityKey", businessKey.Key);
        }
    }
}